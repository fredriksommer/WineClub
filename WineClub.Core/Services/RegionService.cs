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
    public class RegionService : IRegionService
    {

        private readonly HttpClient _httpClient;

        public RegionService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }
        public async Task<RegionDto> CreateRegionAsync(RegionDto region)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"regions", region);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RegionDto>();
        }

        public Task<bool> DeleteRegionAsync(RegionDto regionDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditRegionAsync(RegionDto regionDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RegionDto>> GetRegionsAsync()
        {
            List<RegionDto> items = new List<RegionDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("regions");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<RegionDto>>(content);
            }
            return items;
        }
    }
}
