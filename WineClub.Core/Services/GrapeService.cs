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
    public class GrapeService : IGrapeService
    {

        private readonly HttpClient _httpClient;

        public GrapeService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        public async Task<GrapeDto> CreateGrapeAsync(GrapeDto grape)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"grapes", grape);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<GrapeDto>();
        }

        public Task<bool> DeleteGrapeAsync(GrapeDto grape)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditGrapeAsync(GrapeDto grape)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GrapeDto>> GetGrapesAsync()
        {
            List<GrapeDto> items = new List<GrapeDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("grapes");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<GrapeDto>>(content);
            }
            return items;
        }
    }
}
