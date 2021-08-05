using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using PokerTime.Shared.Entities;
using System.Net;

namespace PokerTime.App.Services
{
    public class EmailService
    {
        public void SendTournamentInviteEmails(List<Invitee> Invitees, Event Event)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(),
                EnableSsl = true
            };
        }

    }
}
