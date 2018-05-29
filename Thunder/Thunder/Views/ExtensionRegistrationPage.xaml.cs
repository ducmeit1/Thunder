using System;
using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtensionRegistrationPage : ContentPage
    {
        private readonly ExtensionRegistrationViewModel _viewModel;
        public ExtensionRegistrationPage()
        {
            BindingContext = _viewModel = new ExtensionRegistrationViewModel();
            _viewModel.SendTokenCommand.Execute(null);
            InitializeComponent();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}