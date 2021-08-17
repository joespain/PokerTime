using PokerTime.App.Interfaces;
using PokerTime.Shared.Email;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class EmailDataService : IEmailDataService
    {
        private readonly HttpClient _httpClient;
        public EmailDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmail(MailRequest email)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"api/email/send", structureJson);
        }
    }




}
