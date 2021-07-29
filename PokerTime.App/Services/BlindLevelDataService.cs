using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class BlindLevelDataService : IBlindLevelDataService
    {
        private readonly HttpClient _httpClient;

        public BlindLevelDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BlindLevel>> GetBlindLevels(int structureId, Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<BlindLevel>>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/tournamentstructures/{structureId}/blindlevels"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<BlindLevel> GetBlindLevel(int structureId, Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<BlindLevel>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/tournamentstructures/{structureId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<BlindLevel> AddBlindLevel(BlindLevel blindLevel, Guid hostId)
        {
            var blindLevelJson = new StringContent(JsonSerializer.Serialize(blindLevel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/users/{hostId}/tournamentstructures/{blindLevel.TournamentStructureId}/blindlevels", blindLevelJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<BlindLevel>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateBlindLevel(BlindLevel blindLevel, Guid hostId)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(blindLevel), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/users/{hostId}/tournamentstructures/{blindLevel.TournamentStructureId}/blindlevels/{blindLevel.Id}", structureJson);
        }

        public async Task DeleteStructure(BlindLevel blindLevel, Guid hostId)
        {
            await _httpClient.DeleteAsync($"api/Users/{hostId}/TournamentStructures/{blindLevel.TournamentStructureId}/blindlevels/{blindLevel.Id}");
        }
    }
}
