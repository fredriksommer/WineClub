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
    public class WineryService : IWineryService
    {
        private readonly HttpClient _httpClient;

        public WineryService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        public async Task<WineryDto> CreateWineryAsync(WineryDto winery)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"wineries", winery);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WineryDto>();
        }

        public Task<bool> DeleteWineryAsync(WineryDto winery)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditWineryAsync(WineryDto winery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WineryDto>> GetWineriesAsync()
        {
            List<WineryDto> items = new List<WineryDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("wineries");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                items = await Json.ToObjectAsync<List<WineryDto>>(content);
            }
            return items;
        }
    }
}
