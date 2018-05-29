using SQLite;
using System.Diagnostics;
using Thunder.Helpers;
using Thunder.IServices;
using Thunder.Models;
using Thunder.Repositories;
using Thunder.Views;
using Xamarin.Forms;

namespace Thunder
{
    public partial class App : Application
    {
        private static SQLiteAsyncConnection _sqliteAsyncConnection;
        public static SQLiteAsyncConnection SqLiteAsyncConnection => _sqliteAsyncConnection;
        private static IUnitOfWork _unitOfWork;
        public static IUnitOfWork UnitOfWork => _unitOfWork;

        public App()
        {
            MessagingCenter.Subscribe<Application, MessageCenterAlert>(this, "Message", async (sender, obj) =>
            {
                await MainPage.DisplayAlert(obj.Title, obj.Message, obj.Cancel);
            });
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Properties["UserLogin"] = string.Empty;
            Init();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void Init()
        {
            if (_sqliteAsyncConnection == null)
                _sqliteAsyncConnection = DependencyService.Get<ISQLite>().GetSqLiteAsyncConnection();
            //Clean();
            Debug.WriteLine("[Log]: Everything are still good so far! Congratulations.");
            if (_unitOfWork == null)
                _unitOfWork = new UnitOfWork(_sqliteAsyncConnection);
        }

        private void Clean()
        {
            _sqliteAsyncConnection.DropTableAsync<User>().Wait();
            _sqliteAsyncConnection.DropTableAsync<Job>().Wait();
        }

        ~App()
        {
            if (_sqliteAsyncConnection != null)
            {
                _sqliteAsyncConnection.GetConnection().Close();
                Debug.WriteLine("[Log]: Closed sqlite connection");
            }
        }
    }
}
