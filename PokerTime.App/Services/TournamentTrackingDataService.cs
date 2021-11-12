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

        public async Task<TournamentStructure> GetTournamentStructure(Guid trackingId, int structureId)
        {
            var TournamentStructureJson = await _httpClient.GetStreamAsync($"api/tournamenttracking/{trackingId}/{structureId}");

            var TournamentStructure = await JsonSerializer.DeserializeAsync<TournamentStructure>(TournamentStructureJson,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return TournamentStructure;
        }



    }
}
