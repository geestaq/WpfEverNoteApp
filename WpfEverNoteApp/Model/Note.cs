using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfEverNoteApp.Model
{
    class Note : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int NotebookId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string FileLocation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
