using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Helpers;
using Thunder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    public class UserLoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class LoginViewModel : BaseSQLiteViewModel
    {
        public UserLoginViewModel User { get; set; }

        public ICommand LoginCommand { get; private set; }
        public ICommand SignupCommand { get; private set; }

        public LoginViewModel()
        {
            IsBusy = false;
            User = new UserLoginViewModel();
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            SignupCommand = new Command(async () => await ExecuteSignupCommand());
        }

        public async Task ExecuteLoginCommand()
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!await Task.FromResult(ValidateLogin()))
            {
                IsBusy = false;
                return;
            }
            var user = await UnitOfWork.Users.FindAsync(model => model.Email == User.Email
                                                       && model.Password == User.Password);
            if (user != null)
            {
                Application.Current.Properties["UserLogin"] = user.Email;
                if (!user.IsVerified)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ExtensionRegistrationPage());
                }
                else
                {
                    Task.WaitAll();
                    await Application.Current.MainPage.Navigation.PushAsync(new JobMarketPage());
                }
            }
            else
            {
                SendDisplayAlert("Error", "Email or Password is incorrect!");
            }
            IsBusy = false;
        }

        public async Task ExecuteSignupCommand()
        {
            if (IsBusy) return;
            await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage());
        }

        private bool ValidateLogin()
        {
            if (Ultilities.StringIsEmptyOrNull(User.Email) || Ultilities.StringIsEmptyOrNull(User.Password))
            {
                SendDisplayAlert("Error", "Email and Password are not allowed empty!");
                return false;
            }
            else if
                (!Ultilities.StringIsEmail(User.Email))
            {
                SendDisplayAlert("Error", "This is not an email!");
                return false;
            }
            return true;
        }
    }
}
