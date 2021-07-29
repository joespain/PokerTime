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
    public class EventDataService : IEventDataService
    {
        private readonly HttpClient _httpClient;

        public EventDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Event>> GetEvents(Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Event>>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/events"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Event> GetEvent(int eventId, Guid hostId)
        {
            return await JsonSerializer.DeserializeAsync<Event>(
                await _httpClient.GetStreamAsync($"api/users/{hostId}/events/{eventId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Event> AddEvent(Event newEvent, Guid hostId)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(newEvent), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/users/{hostId}/events/", structureJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Event>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateEvent(Event updateEvent)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(updateEvent), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/users/{updateEvent.HostId}/events/{updateEvent.Id}", structureJson);
        }

        public async Task DeleteEvent(int eventId, Guid hostId)
        {
            await _httpClient.DeleteAsync($"api/users/{hostId}/events/{eventId}");
        }
    }
}
