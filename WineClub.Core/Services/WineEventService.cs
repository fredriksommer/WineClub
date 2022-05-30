using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WineClub.Core.Constants;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Core.Helpers;

namespace WineClub.Core.Services
{
    public class WineEventService : IWineEventService
    {
        private readonly HttpClient _httpClient;

        public WineEventService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        public async Task<WineEventDto> AddWineEventWine(WineEventDto wineEvent, WineDto wine)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"WineEvents/{wineEvent.WineEventId}/wine", wine);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineEventDto>();
        }

        public async Task<bool> DeleteWineEventWine(WineEventDto wineEvent, WineDto wine)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"WineEvents/{wineEvent.WineEventId}/wine/{wine.WineId}");
            return response.IsSuccessStatusCode;
        }


        public async Task<WineEventDto> CreateWineEventAsync(WineEventDto wineEvent)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wineevents", wineEvent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineEventDto>();
        }

        public async Task<bool> DeleteWineEventAsync(WineEventDto wineEvent)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"WineEvents/{wineEvent.WineEventId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditWineEventAsync(WineEventDto wineEvent)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"WineEvents/{wineEvent.WineEventId}", wineEvent);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<WineEventDto>> GetWineEventsAsync()
        {
            List<WineEventDto> items = new List<WineEventDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("wineevents");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineEventDto>>(content);
            }
            return items;
        }

        public async Task<WineEventDto> AddWineEventUser(WineEventDto wineEvent, UserDto user)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"WineEvents/{wineEvent.WineEventId}/user", user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineEventDto>();
        }

        public async Task<bool> DeleteWineEventUser(WineEventDto wineEvent, UserDto user)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"WineEvents/{wineEvent.WineEventId}/user/{user.UserId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<WineEventDto>> GetNextWineEventAsync()
        {
            List<WineEventDto> items = new List<WineEventDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("wineevents/nextevent");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineEventDto>>(content);
            }
            return items;
        }
    }
}
