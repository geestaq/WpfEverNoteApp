using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfEverNoteApp.Model;
using WpfEverNoteApp.ViewModel.Commands;

namespace WpfEverNoteApp.ViewModel
{
    public class NotesVM
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                //TODO: Pobranie notatek
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }

        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            CreateTables();
            ReadNotebooks();
        }

        /// <summary>
        /// Tworzy tabele w bazie danych
        /// </summary>
        private void CreateTables()
        {
            using (SQLite.SQLiteConnection cn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                cn.CreateTable<Notebook>();
                cn.CreateTable<Note>();
            }
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
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook"
            };

            DatabaseHelper.Insert(newNotebook);
        }

        public void ReadNotebooks()
        {
            using (SQLite.SQLiteConnection cn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                var notebooks = cn.Table<Notebook>().ToList();

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
    }
}
