using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Models;
using Xamarin.Forms;

namespace Thunder.ViewModels
{
    public class UserViewModel : BaseSQLiteViewModel
    {
        private User _user;

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ICommand LoadUserDetailCommand { get; private set; }


        public UserViewModel(int userId)
        {
            LoadUserDetailCommand = new Command<int>(async id => await ExecuteLoadUserDetailCommand(id));
            LoadUserDetailCommand.Execute(userId);
        }

        private async Task ExecuteLoadUserDetailCommand(int id)
        {
            User = await UnitOfWork.Users.GetAsync(id);
        }
    }
}
