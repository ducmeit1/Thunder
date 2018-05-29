using System;
using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private readonly RegistrationViewModel _viewModel;
        public RegistrationPage()
        {
            BindingContext = _viewModel = new RegistrationViewModel();
            InitializeComponent();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}