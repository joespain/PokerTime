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

        public async Task<IEnumerable<Invitee>> GetInvitees(Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Invitee>>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/invitees"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Invitee> GetInvitee(int inviteeId, Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<Invitee>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/invitees/{inviteeId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Invitee> AddInvitee(Invitee invitee, Guid hostId)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(invitee), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/users/{hostId}/invitees/", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Invitee>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateInvitee(Invitee invitee)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(invitee), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/users/{invitee.HostId}/invitees/{invitee.Id}", structureJson);
        }

        public async Task DeleteInvitee(int inviteeId, Guid hostId)
        {
            await _httpClient.DeleteAsync($"api/users/{hostId}/invitees/{inviteeId}");
        }
    }
}
