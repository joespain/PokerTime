using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class TournamentEventDataService : ITournamentEventDataService
    {
        private readonly HttpClient _httpClient;
        public TournamentEventDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TournamentTracking> AddTournamentTracking(TournamentTracking tracking)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(tracking), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/tournamentevent", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<TournamentTracking>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateTournamentTracking(TournamentTracking tracking)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(tracking), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/tournamentevent/{tracking.Id}", structureJson);
        }

        public async Task EndTournamentTracking(TournamentTracking tracking)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(tracking), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"api/tournamentevent/{tracking.Id}/end", structureJson);

        }

        public async Task<TournamentTracking> GetTournamentTracking(Guid trackingId)
        {
            var TournamentTrackerJson = await _httpClient.GetStreamAsync($"api/tournamentevent/{trackingId}");

            TournamentTracking TournamentTracker = await JsonSerializer.DeserializeAsync<TournamentTracking>(TournamentTrackerJson,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return TournamentTracker;

        }
    }
}
