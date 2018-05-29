using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewJobPage : ContentPage
    {
        private readonly ViewJobViewModel _viewModel;
        public ViewJobPage(int id)
        {
            BindingContext = _viewModel = new ViewJobViewModel(id);
            InitializeComponent();
        }

        public ViewJobPage()
        {
            InitializeComponent();
        }
    }
}