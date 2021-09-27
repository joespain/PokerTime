
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PokerTime.Shared.Email;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IdentityServer
{

    public class EmailSender : IEmailSender
    {

        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailRequest mailRequest = new() {
                Body = htmlMessage,
                ToEmail = email,
                Subject = subject
            };

            var emailMessage = new MimeMessage();
            emailMessage.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            emailMessage.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            emailMessage.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            emailMessage.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(emailMessage);
            smtp.Disconnect(true);

            return;
        }

    }
}
