using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class TournamentTrackingDataService : ITournamentTrackingDataService
    {
        private readonly HttpClient _httpClient;
        public TournamentTrackingDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TournamentTracking> GetTournamentTracking(Guid trackingId)
        {
            var TournamentTrackerJson = await _httpClient.GetStreamAsync($"api/tournamenttracking/{trackingId}");
            
            var TournamentTracker = await JsonSerializer.DeserializeAsync<TournamentTracking>(TournamentTrackerJson,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return TournamentTracker;
        }

        public async Task<TournamentTracking> AddTournamentTracking(TournamentTracking tracking)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(tracking), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/tournamenttracking", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<TournamentTracking>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateTournamentTracking(TournamentTracking tracking)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(tracking), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/tournamenttracking/{tracking.Id}", structureJson);
        }


    }
}
