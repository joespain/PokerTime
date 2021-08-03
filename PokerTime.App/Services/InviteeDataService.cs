using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class InviteeDataService : IInviteeDataService
    {
        private readonly HttpClient _httpClient;

        public InviteeDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Invitee>> GetInvitees()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Invitee>>(
                await _httpClient.GetStreamAsync($"api/invitees"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Invitee> GetInvitee(int inviteeId)
        {
            return await JsonSerializer.DeserializeAsync<Invitee>(
                await _httpClient.GetStreamAsync($"api/invitees/{inviteeId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Invitee> AddInvitee(Invitee invitee)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(invitee), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/invitees/", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Invitee>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateInvitee(Invitee invitee)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(invitee), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/invitees/{invitee.Id}", structureJson);
        }

        public async Task DeleteInvitee(int inviteeId)
        {
            await _httpClient.DeleteAsync($"api/invitees/{inviteeId}");
        }
    }
}
