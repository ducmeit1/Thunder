using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Thunder.Helpers;
using Thunder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thunder.ViewModels
{
    public class PublicJobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Reward { get; set; }
        public int UserId { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class JobMarketViewModel : BaseSQLiteViewModel
    {
        private bool _orderBy = true;

        public bool OrderBy
        {
            get { return _orderBy; }
            set {
                SetProperty(ref _orderBy, value);
                SearchCommand.Execute(null);
            }
        }

        private bool _myTask = false;

        public bool MyTask
        {
            get { return _myTask; }
            set {
                SetProperty(ref _myTask, value);
                SearchCommand.Execute(null);
            }
        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set {
                SetProperty(ref _isRefreshing, value);
            }
        }

        private string _keyword = string.Empty;

        public string Keyword
        {
            get { return _keyword; }
            set {
                SetProperty(ref _keyword, value);
                SearchCommand.Execute(null);
            }
        }

        private double _minRange = 0;

        public double MinRange
        {
            get { return _minRange; }
            set {
                SetProperty(ref _minRange, value);
                if (MinRange < MaxRange || MaxRange == 0)
                    SearchCommand.Execute(null);
            }
        }

        private double _maxRange = 0;

        public double MaxRange
        {
            get { return _maxRange; }
            set {
                SetProperty(ref _maxRange, value);
                if (MaxRange > MinRange || MinRange == 0)
                    SearchCommand.Execute(null);
            }
        }


        public ObservableRangeCollection<PublicJobViewModel> Jobs { get; set; }

        private readonly string _currentEmail = Application.Current.Properties["UserLogin"] as string;

        public ICommand SearchCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand ViewJobCommand { get; private set; }

        public JobMarketViewModel()
        {
            IsBusy = false;
            Jobs = new ObservableRangeCollection<PublicJobViewModel>();
            SearchCommand = new Command(async () => await ExecuteSearchCommand(), () => !IsBusy);
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand(), () => !IsRefreshing);
            ViewJobCommand = new Command<int>(async id => await ExecuteViewJobCommand(id));
        }

        private async Task ExecuteViewJobCommand(int id)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewJobPage(id));
        }

        private async Task ExecuteRefreshCommand()
        {
            IsRefreshing = true;
            var jobsList = await SearchJob();

            if (Jobs.Any())
                Jobs.Clear();
            if (jobsList != null)
                Jobs.AddRange(jobsList);
            IsRefreshing = false;
        }

        private async Task ExecuteSearchCommand()
        {
            IsBusy = true;
            var jobsList = await SearchJob();

            if (Jobs.Any())
                Jobs.Clear();
            if (jobsList != null)
                Jobs.AddRange(jobsList);
            IsBusy = false;
        }

        private async Task<IEnumerable<PublicJobViewModel>> SearchJob()
        {
            var user = await UnitOfWork.Users.FindAsync(p => p.Email == _currentEmail);
            var query = UnitOfWork.Jobs.AsQueryable();
            query = query.Where(p => p.IsFinished == false && p.DateStart >= DateTime.Today);
            if (MinRange > 0 && MaxRange > 0 && MinRange < MaxRange)
            {
                query = query.Where(p => p.Reward >= MinRange && p.Reward <= MaxRange);
            }
            else if (MinRange > 0)
            {
                query = query.Where(p => p.Reward >= MinRange);
            }
            else if (MaxRange > 0)
            {
                query = query.Where(p => p.Reward <= MaxRange);
            }

            if (!Ultilities.StringIsEmptyOrNull(Keyword))
            {
                query = query.Where(p => p.Name.ToLower().Contains(Keyword.ToLower()));
            }
            if (_myTask)
            {
                query = query.Where(p => p.UserId == user.Id);
            }
            if (_orderBy)
            {
                query = query.OrderBy(p => p.Reward);
            }
            else
            {
                query = query.OrderByDescending(p => p.Reward);
            }
            var list = await query.ToListAsync();
            if (list.Count > 0)
            {
                var jobsList = list.Select(j => new PublicJobViewModel()
                {
                    Id = j.Id,
                    Name = j.Name,
                    Reward = j.Reward,
                    UserId = j.UserId
                }).ToArray();
                return jobsList;
            }
            return null;
        }
    }
}
