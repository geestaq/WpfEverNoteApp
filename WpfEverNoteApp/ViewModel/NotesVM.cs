using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfEverNoteApp.Model;
using WpfEverNoteApp.ViewModel.Commands;

namespace WpfEverNoteApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public string NotesPath => Path.Combine(Environment.CurrentDirectory, "notes");

        private bool isNotebookEditing;

        public bool IsNotebookEditing
        {
            get { return isNotebookEditing; }
            set
            {
                isNotebookEditing = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                ReadNotes();
            }
        }

        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                SelectedNoteChanged(this, new EventArgs());
            }
        }


        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public BeginEditCommand BeginEditCommand { get; set; }
        public RenameNotebookCommand RenameNotebookCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            IsNotebookEditing = false;

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            RenameNotebookCommand = new RenameNotebookCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            CreateTables();
            ReadNotebooks();
            ReadNotes();
        }

        public void OnPropertyChanged([CallerMemberName] string properyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }

        /// <summary>
        /// Tworzy potrzebne tabele w bazie danych
        /// Tworzy folder do zapisu tresci notatek
        /// </summary>
        private void CreateTables()
        {
            using (SQLite.SQLiteConnection cn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                cn.CreateTable<Notebook>();
                cn.CreateTable<Note>();
            }

            //utworzenie folderu na notatki
            if (!Directory.Exists(NotesPath))
                Directory.CreateDirectory(NotesPath);
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert(newNote);

            ReadNotes();
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
                UserId = int.Parse(App.UserId)
            };

            DatabaseHelper.Insert(newNotebook);

            ReadNotebooks();
        }

        public void ReadNotebooks()
        {
            using (SQLite.SQLiteConnection cn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                var notebooks = cn.Table<Notebook>().ToList();
                //TODO: pobieranie notatnikow dla zalogowanego usera

                Notebooks.Clear();
                foreach (var item in notebooks)
                {
                    Notebooks.Add(item);
                }
            }
        }

        public void ReadNotes()
        {
            using (SQLite.SQLiteConnection cn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                if(SelectedNotebook != null)
                {
                    var notes = cn.Table<Note>().Where(x => x.NotebookId == SelectedNotebook.Id).ToList();

                    Notes.Clear();
                    foreach (var item in notes)
                    {
                        Notes.Add(item);
                    }
                }
            }
        }

        public void StartEditing()
        {
            IsNotebookEditing = true;
        }

        public void RenameNotebook(Notebook item)
        {
            if(item != null)
            {
                DatabaseHelper.Update(item);
                IsNotebookEditing = false;
                ReadNotebooks();
            }
        }

        public void UpdateSelectedNote()
        {
            DatabaseHelper.Update(SelectedNote);
        }

    }
}
