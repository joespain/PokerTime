using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}
