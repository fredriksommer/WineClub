using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WineClub.Core.Constants;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;

namespace WineClub.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly HttpClient _httpClient;

        public RatingService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        public async Task<RatingDto> CreateRatingAsync(RatingDto rating)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"ratings", rating);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RatingDto>();
        }
    }
}
