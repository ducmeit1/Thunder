using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Helpers;
using Thunder.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    public class JobViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = String.Empty;
        public string Note { get; set; } = string.Empty;
        public double Reward { get; set; } = 0;
        public DateTime DateStart { get; set; } = DateTime.Now;
        public DateTime DateEnd { get; set; } = DateTime.Now;
        public TimeSpan TimeDeadLine { get; set; } = new TimeSpan(12, 0, 0);
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class AddJobViewModel : BaseSQLiteViewModel
    {
        public JobViewModel Job { get; set; }

        public DateTime MinDateTime { get; set; } = DateTime.Today;

        public ICommand SaveJobCommand { get; private set; }

        public AddJobViewModel()
        {
            IsBusy = false;
            Job = new JobViewModel();
            SaveJobCommand = new Command(async () => await ExecuteSaveJobCommand());
        }

        private async Task ExecuteSaveJobCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (!ValidateJob())
            {
                IsBusy = false;
                return;
            }
            await SaveJobAsync();
            IsBusy = false;

        }

        private async Task SaveJobAsync()
        {
            var currentEmail = Application.Current.Properties["UserLogin"] as string;
            var user = await UnitOfWork.Users.FindAsync(p => p.Email == currentEmail);
            if (user == null)
            {
                SendDisplayAlert("Error", "Bad Request!");
                await Application.Current.MainPage.Navigation.PopToRootAsync(true);
            }
            else
            {
                var job = new Job()
                {
                    Name = Job.Name,
                    Address = Job.Address,
                    DateStart = Job.DateStart,
                    DateEnd = Job.DateEnd,
                    Description = Job.Description,
                    IsFinished = false,
                    Note = Job.Note,
                    Reward = Job.Reward,
                    TimeDeadLine = Job.TimeDeadLine,
                    UserId = user.Id
                };
                await UnitOfWork.Jobs.Add(job);
                SendDisplayAlert("Successfully", "Added new task");
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }
        }

        private bool ValidateJob()
        {
            if (Ultilities.StringIsEmptyOrNull(Job.Name))
            {
                SendDisplayAlert("Error", "Name is not allowed empty!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(Job.Description))
            {
                SendDisplayAlert("Error", "Description is not allowed empty!");
                return false;
            }
            if (Ultilities.StringIsEmptyOrNull(Job.Address))
            {
                SendDisplayAlert("Error", "Address is not allowed empty!");
                return false;
            }
            if (Job.DateEnd < Job.DateStart)
            {
                SendDisplayAlert("Error", "Date Start have to less than Date End");
                return false;
            }
            if (Job.DateEnd == Job.DateStart)
            {
                if (Job.TimeDeadLine < DateTime.Now.TimeOfDay)
                {
                    SendDisplayAlert("Error", "Please choose another deadline!");
                    return false;
                }
            }
            return true;
        }
    }
}
