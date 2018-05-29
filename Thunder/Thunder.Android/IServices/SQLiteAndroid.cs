using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using Thunder.Droid.IServices;
using Thunder.IServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace Thunder.Droid.IServices
{
    public class SQLiteAndroid : ISQLite
    {
        private string GetDatabasePath()
        {
            var fileName = "thunder.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fileFullPath = Path.Combine(documentsPath, fileName);
            Debug.WriteLine($"[Log]: Full file path database from iOS device: {fileFullPath}");
            return fileFullPath;
        }

        public SQLiteAsyncConnection GetSqLiteAsyncConnection() => new SQLiteAsyncConnection(GetDatabasePath());
    }
}