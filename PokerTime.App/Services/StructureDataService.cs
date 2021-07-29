using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class StructureDataService : IStructureDataService
    {
        private readonly HttpClient _httpClient;

        public StructureDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<TournamentStructure>> GetStructures(Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<TournamentStructure>>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/tournamentstructures"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<TournamentStructure> GetStructure(int structureId, Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<TournamentStructure>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/tournamentstructures/{structureId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<TournamentStructure> AddStructure(TournamentStructure structure, Guid hostId)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(structure), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/users/{hostId}/tournamentstructures/", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<TournamentStructure>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateStructure(TournamentStructure structure)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(structure), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/users/{structure.HostId}/tournamentstructures/{structure.Id}", structureJson);
        }

        public async Task DeleteStructure(int structureId, Guid hostId)
        {
            await _httpClient.DeleteAsync($"api/users/{hostId}/tournamentstructures/{structureId}");
        }
    }
}
