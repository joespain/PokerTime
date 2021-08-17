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

        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Event>>(
                await _httpClient.GetStreamAsync($"api/events"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Event> GetEvent(Guid eventId)
        {
            var gottenEvent = await JsonSerializer.DeserializeAsync<Event>(
                await _httpClient.GetStreamAsync($"api/events/{eventId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return gottenEvent;
        }

        public async Task<Event> AddEvent(Event newEvent)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(newEvent), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/events/", structureJson);

            if (response.IsSuccessStatusCode)
            {
                var returnedEvent = await JsonSerializer.DeserializeAsync<Event>(await response.Content.ReadAsStreamAsync(), 
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return returnedEvent;
            }
            return null;
        }

        public async Task UpdateEvent(Event updateEvent)
        {
            var structureJson = new StringContent(JsonSerializer.Serialize(updateEvent), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/events/{updateEvent.Id}", structureJson);
        }

        public async Task DeleteEvent(Guid eventId)
        {
            await _httpClient.DeleteAsync($"api/events/{eventId}");
        }
    }
}
