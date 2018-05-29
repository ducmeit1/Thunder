using System.Threading.Tasks;

namespace Thunder.IServices
{
    public interface IHelper
    {
        Task<bool> SendEmailAsync(string to, string title, string message);
    }
}
