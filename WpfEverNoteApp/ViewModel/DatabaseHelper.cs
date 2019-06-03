using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEverNoteApp.ViewModel
{
    public class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3"); 

        public static bool Insert<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection cn = new SQLiteConnection(dbFile))
            {
                cn.CreateTable<T>();
                int rowsCount = cn.Insert(item);
                if (rowsCount > 0)
                    result = true;
            }

            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection cn = new SQLiteConnection(dbFile))
            {
                cn.CreateTable<T>();
                int rowsCount = cn.Update(item);
                if (rowsCount > 0)
                    result = true;
            }

            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection cn = new SQLiteConnection(dbFile))
            {
                cn.CreateTable<T>();
                int rowsCount = cn.Delete(item);
                if (rowsCount > 0)
                    result = true;
            }

            return result;
        }

    }
}
