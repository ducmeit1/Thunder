using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        private readonly UserViewModel _viewModel;
        public UserPage(int id)
        {
            BindingContext = _viewModel = new UserViewModel(id);
            InitializeComponent();
        }

        public UserPage()
        {
            InitializeComponent();
        }
    }
}