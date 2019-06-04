using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfEverNoteApp.Model;
using WpfEverNoteApp.ViewModel.Commands;

namespace WpfEverNoteApp.ViewModel
{
    public class LoginVM
    {
        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public event EventHandler HasLoggedIn;

        public LoginVM()
        {
            User = new User();
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);

            CreateTables();
        }

        /// <summary>
        /// Tworzy potrzebne tabele w bazie danych
        /// </summary>
        private void CreateTables()
        {
            using (SQLiteConnection cn = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                cn.CreateTable<User>();
            }
        }

        public void Login()
        {
            using (SQLiteConnection cn = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                var user = cn.Table<User>().Where(x => x.Username == User.Username).FirstOrDefault();

                if(user != null && user.Password == User.Password)
                {
                    App.UserId = user.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }
            }

        }

        public void Register()
        {
            using (SQLiteConnection cn = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                var result = DatabaseHelper.Insert(User);

                if(result)
                {
                    App.UserId = User.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }
            }
        }


    }
}
