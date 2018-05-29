using System;
using Thunder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobMarketPage : ContentPage
    {
        private readonly JobMarketViewModel _viewModel;
        public JobMarketPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            Title = "Task Market";
            BindingContext = _viewModel = new JobMarketViewModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.RefreshCommand.Execute(null);
            base.OnAppearing();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listView = (ListView)sender;
            var job = e.Item as PublicJobViewModel;
            var id = job.Id;
            listView.SelectedItem = null;
            _viewModel.ViewJobCommand.Execute(id);
        }

        private async void NewTask_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddJobPage());
        }
    }
}