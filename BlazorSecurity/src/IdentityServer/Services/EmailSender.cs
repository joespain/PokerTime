using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PokerTime.Shared.Email;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityServer.Services
{

    public class EmailSender : IEmailSender
    {

        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using(var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email, email));
                message.From = new MailAddress("pokertimeapp@gmail.com", "PokerTime");
                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("pokertimeapp@gmail.com", "GreatestApp99*");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }

            return Task.CompletedTask;
        }

    }
}
