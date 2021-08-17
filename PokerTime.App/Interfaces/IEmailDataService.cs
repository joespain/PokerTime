using PokerTime.Shared.Email;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IEmailDataService
    {
        Task SendEmail(MailRequest email);
    }
}