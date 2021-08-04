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
    public class HostDataService : IHostDataService
    {
        private readonly HttpClient _httpClient;

        public HostDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Host> GetHost()
        {
            return await JsonSerializer.DeserializeAsync<Host>(
                await _httpClient.GetStreamAsync($"api/hosts"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Host> AddHost(Host host)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(host), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/hosts", userJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Host>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateHost(Host host)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(host), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/hosts/", userJson);
        }

        public async Task DeleteHost()
        {
            await _httpClient.DeleteAsync($"api/hosts");
        }

    }
}
