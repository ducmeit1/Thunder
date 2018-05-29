using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Helpers;
using Thunder.Models;
using Thunder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    public class RegisterViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RegistrationViewModel : BaseSQLiteViewModel
    {
        public RegisterViewModel User { set; get; }

        public ICommand RegistrationCommand { get; private set; }

        public RegistrationViewModel()
        {
            IsBusy = false;
            Title = "Registration";
            User = new RegisterViewModel();
            RegistrationCommand = new Command(async () => await ExecuteRegistrationCommand());
        }



        private async Task ExecuteRegistrationCommand()
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!await Task.FromResult<bool>(ValidateUser()))
            {
                IsBusy = false;
                return;
            }
            var user = await UnitOfWork.Users.FindAsync(model => model.Email == User.Email);
            if (user != null)
            {
                SendDisplayAlert("Error", "This email is already exist! Please choose another email.");
            }
            else
            {
                var newUser = new User()
                {
                    Name = User.Name,
                    Email = User.Email,
                    Password = User.Password,
                    Address = User.Address,
                    ContactNumber = User.ContactNumber,
                    IsVerified = false,
                    Money = 0
                };
                int rowCount = await UnitOfWork.Users.Add(newUser);
                if (rowCount > 0)
                {
                    Application.Current.Properties["UserLogin"] = newUser.Email;
                    await Application.Current.MainPage.Navigation.PopModalAsync(true);
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ExtensionRegistrationPage(), false);
                }
                else
                {
                    SendDisplayAlert("Error", "Unsuccessful, something is wrong!");
                }
            }
            IsBusy = false;
        }

        private bool ValidateUser()
        {
            if (Ultilities.StringIsEmptyOrNull(User.Name))
            {
                SendDisplayAlert("Error", "Name is not allowed empty!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(User.Email))
            {
                SendDisplayAlert("Error", "Email is not allowed empty!");
                return false;
            }
            else if (!Ultilities.StringIsEmail(User.Email))
            {
                SendDisplayAlert("Error", "That is not email address!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(User.Password))
            {
                SendDisplayAlert("Error", "Password is not allowed empty!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(User.Address))
            {
                SendDisplayAlert("Error", "Address is not allowed empty!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(User.ContactNumber))
            {
                SendDisplayAlert("Error", "Contact Number is not allowed empty!");
                return false;
            }
            if (!Ultilities.StringIsPhoneNumber(User.ContactNumber))
            {
                SendDisplayAlert("Error", "That is not a contact number");
                return false;
            }
            return true;
        }
    }
}
