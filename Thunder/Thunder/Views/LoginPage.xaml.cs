using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _viewModel = new LoginViewModel();
            InitializeComponent();
        }
    }
}