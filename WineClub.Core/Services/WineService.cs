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
    public class WineService : IWineService
    {
        private readonly HttpClient _httpClient;

        public WineService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        /// <summary>
        /// Add many to many Wine - Add grape
        /// </summary>
        /// <param name="wine"></param>
        /// <param name="grape"></param>
        /// <returns></returns>
        public async Task<WineDto> AddWineGrape(WineDto wine, GrapeDto grape)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wines/{wine.WineId}/Grape", grape);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineDto>();
        }

        public async Task<WineDto> AddWineRating(WineDto wine, RatingDto rating)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wines/{wine.WineId}/Rating", rating);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineDto>();
        }

        public async Task<WineDto> AddWineRegion(WineDto wine, RegionDto region)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wines/{wine.WineId}/Region", region);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineDto>();
        }

        public async Task<WineDto> CreateWineAsync(WineDto wine)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wines", wine);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineDto>();
        }

        public async Task<bool> DeleteWineAsync(WineDto wine)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"wines/{wine.WineId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteWineGrape(WineDto wine, GrapeDto grape)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"wines/{wine.WineId}/Grape/{grape.GrapeId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteWineRegion(WineDto wine, RegionDto region)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"wines/{wine.WineId}/Region/{region.RegionId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditWineAsync(WineDto wine)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Wines/{wine.WineId}", wine);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<WineDto>> GetTop3WinesAsync()
        {
            List<WineDto> items = new List<WineDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("wines/Top3");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineDto>>(content);
            }
            return items;
        }

        public async Task<IEnumerable<WineDto>> GetWinesAsync()
        {
            List<WineDto> items = new List<WineDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("wines?includeGrapes=true&includeRegions=true");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineDto>>(content);
            }
            return items;
        }

        public async Task<IEnumerable<WineDto>> GetWinesRatedByUserIdAsync(int userId)
        {
            List<WineDto> items = new List<WineDto>();
            HttpResponseMessage response = await _httpClient.GetAsync($"wines/RatedByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineDto>>(content);
            }
            return items;
        }
    }
}
