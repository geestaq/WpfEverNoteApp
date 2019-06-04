using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfEverNoteApp.Model;

namespace WpfEverNoteApp.ViewModel.Commands
{
    public class RenameNotebookCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public RenameNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Notebook notebook = parameter as Notebook;
            VM.RenameNotebook(notebook);
        }
    }
}
