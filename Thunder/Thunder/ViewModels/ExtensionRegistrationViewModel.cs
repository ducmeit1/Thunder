using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Helpers;
using Thunder.IServices;
using Thunder.Models;
using Thunder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ExtensionRegistrationViewModel : BaseSQLiteViewModel
    {
        public string Token { get; set; } = string.Empty;

        public ICommand SendTokenCommand { get; private set; }
        public ICommand ComfirmEmailAddressCommand { get; private set; }

        private readonly string _currentEmail = Application.Current.Properties["UserLogin"] as string;

        private string _currentToken = "";

        private readonly IHelper _helper = DependencyService.Get<IHelper>();

        public ExtensionRegistrationViewModel()
        {
            IsBusy = false;
            SendTokenCommand = new Command(async () => await ExecuteSendTokenCommand());
            ComfirmEmailAddressCommand = new Command(async () => await ExecuteConfirmEmailAddressCommand());
        }

        private async Task ExecuteSendTokenCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            var user = await UnitOfWork.Users.FindAsync(prop => prop.Email == _currentEmail);
            if (user == null)
            {
                SendDisplayAlert("Error", "This email is not existed!");
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }
            else
            {
                if (user.IsVerified)
                {
                    SendDisplayAlert("Error", "This email is verified!");
                    await Application.Current.MainPage.Navigation.PopModalAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new JobMarketPage(), true);
                }
                else
                {
                    await SendEmailAsync(user);
                }
            }
            IsBusy = false;
        }

        private async Task ExecuteConfirmEmailAddressCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (!await Task.FromResult(ValidateToken()))
            {
                IsBusy = false;
                return;
            }
            if (await ConfirmEmailAddressAsync())
            {
                await Application.Current.MainPage.Navigation.PopModalAsync(false);
                await Application.Current.MainPage.Navigation.PushAsync(new JobMarketPage());
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }
            IsBusy = false;
        }

        private bool ValidateToken()
        {
            if (Ultilities.StringIsEmptyOrNull(Token))
            {
                SendDisplayAlert("Error", "Please enter the token number!");
                return false;
            }
            if (Token != _currentToken)
            {
                SendDisplayAlert("Error", "This is invalid token, resend token to try again!");
                return false;
            }
            return true;
        }

        private async Task<bool> ConfirmEmailAddressAsync()
        {
            var user = await UnitOfWork.Users.FindAsync(prop => prop.Email == _currentEmail);
            if (user != null)
            {
                user.IsVerified = true;
                await UnitOfWork.Users.Update(user);
                Application.Current.Properties["CurrentToken"] = string.Empty;
                SendDisplayAlert("Successful", "Your email has been verified!");
                return true;
            }
            SendDisplayAlert("Error", "This email is not existed!");
            return false;
        }

        private async Task SendEmailAsync(User user)
        {
            Application.Current.Properties["CurrentToken"] = _currentToken = Ultilities.RandomNumber(6);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<p>Dear {user.Name}, <br/>");
            sb.AppendLine($"This is the token to verify your email: <b>{_currentToken}</b>. <br/>");
            sb.AppendLine("Please do follow instruction to active your email. <br/>");
            sb.AppendLine("Thank you. <br/>");
            sb.AppendLine("--------------------------------------- <br/>");
            sb.AppendLine("Best regards from Thunder Market. </p>");
            string messageBody = sb.ToString();
            if (await _helper.SendEmailAsync(_currentEmail, "Verification Account - Thunder Market", messageBody))
            {
                SendDisplayAlert("Notification", "An email sent to your inbox!");
                return;
            }
            SendDisplayAlert("Error", "We can't send email to your email address, click resend to try again!");
        }
    }
}
