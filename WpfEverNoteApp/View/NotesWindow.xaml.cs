using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfEverNoteApp.ViewModel;

namespace WpfEverNoteApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM VM;
        public NotesWindow()
        {
            InitializeComponent();

            VM = new NotesVM();
            container.DataContext = VM;

            VM.SelectedNoteChanged += VM_SelectedNoteChanged;
        }

        private void VM_SelectedNoteChanged(object sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if(!string.IsNullOrEmpty(VM.SelectedNote.FileLocation))
            {
                FileStream fs = new FileStream(VM.SelectedNote.FileLocation, FileMode.Open);
                TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ContentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int charsCount = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Length;
            statusTextBlock.Text = $"Document length: {charsCount}";
        }

        private void BoldBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isBtnEnabled = (sender as ToggleButton).IsChecked ?? false;

            if(isBtnEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void ContentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldBtn.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(FontWeights.Bold));
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if(string.IsNullOrEmpty(App.UserId))
            {
                var loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            string rtfFile = System.IO.Path.Combine(VM.NotesPath, $"{VM.SelectedNote.Id}.rtf");
            VM.SelectedNote.FileLocation = rtfFile;

            FileStream fs = new FileStream(VM.SelectedNote.FileLocation, FileMode.Create);
            TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
            range.Save(fs, DataFormats.Rtf);

            VM.UpdateSelectedNote();

        }
    }
}
