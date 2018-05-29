using MvvmHelpers;
using Thunder.Helpers;
using Thunder.IServices;
using Thunder.Repositories;
using Xamarin.Forms;

namespace Thunder.ViewModels
{
    public class BaseSQLiteViewModel : BaseViewModel
    {
        protected IUnitOfWork UnitOfWork
        {
            get {
                return App.UnitOfWork as UnitOfWork;
            }
        }

        protected void SendDisplayAlert(string title, string message)
        {
            MessagingCenter.Send(Application.Current, "Message", new MessageCenterAlert(title, message));
        }
    }
}
