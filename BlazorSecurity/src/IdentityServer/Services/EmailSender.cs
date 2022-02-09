using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokerTime.Shared.Email;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace IdentityServer.Services
{

    public class EmailSender : IEmailSender
    {

        private readonly IMailService _mailService;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IMailService mailService, ILogger<EmailSender> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailRequest emailRequest = new()
                {
                    ToEmail = email,
                    Subject = subject,
                    Body = htmlMessage
                };

                await _mailService.SendEmailAsync(emailRequest);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Error {e.Message}");
            }

            //_logger.LogInformation(message:"Sending user confirmation email to {emailAddress}", email);
            //using (var message = new MailMessage())
            //{
            //    message.To.Add(new MailAddress(email, email));
            //    message.From = new MailAddress("pokertimeapp@gmail.com", "PokerTime");
            //    message.Subject = subject;
            //    message.Body = htmlMessage;
            //    message.IsBodyHtml = true;

            //    using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
            //    {
            //        client.Port = 587;
            //        client.UseDefaultCredentials = false;
            //        client.Credentials = new NetworkCredential("pokertimeapp@gmail.com", "GreatestApp99*");
            //        client.EnableSsl = true;
            //        client.Send(message);
            //    }
            //}

            //return Task.CompletedTask;
        }

    }
}
