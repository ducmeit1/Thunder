using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Models;
using Thunder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ViewJobViewModel : BaseSQLiteViewModel
    {
        private Job _job;

        public Job Job
        {
            get { return _job; }
            set { SetProperty(ref _job, value); }
        }

        private bool _canFinish = false;

        public bool CanFinish
        {
            get { return _canFinish; }
            set { SetProperty(ref _canFinish, value); }
        }

        public ICommand GetJobDetailCommand { get; private set; }
        public ICommand ContactCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public ViewJobViewModel(int jobId)
        {
            GetJobDetailCommand = new Command<int>(async id => await ExecuteGetJobDetailCommand(id));
            ContactCommand = new Command<int>(async id => await ExecuteContactCommand(id));
            FinishCommand = new Command(async () => await ExecuteFinishCommand());
            GetJobDetailCommand.Execute(jobId);
        }

        private async Task ExecuteGetJobDetailCommand(int id)
        {
            Job = await UnitOfWork.Jobs.GetAsync(id);
            var currentEmail = Application.Current.Properties["UserLogin"] as string;
            var user = await UnitOfWork.Users.FindAsync(p => p.Email == currentEmail);
            if (user.Id == Job.UserId)
            {
                CanFinish = true;
            }
        }

        public async Task ExecuteFinishCommand()
        {
            var task = Job;
            task.IsFinished = true;
            await UnitOfWork.Jobs.Remove(Job);
            await UnitOfWork.Jobs.Update(task);
            SendDisplayAlert("Successfully", "Updated this task!");
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }

        private async Task ExecuteContactCommand(int id)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new UserPage(id));
        }
    }
}
