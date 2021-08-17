using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.Shared.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
