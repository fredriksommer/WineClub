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
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress.Api) };
        }

        public async Task<UserDto> CreateUserAsync(UserDto user)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"users", user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public Task<bool> DeleteUserAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditUserAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            List<UserDto> users = new List<UserDto>();
            HttpResponseMessage response = await _httpClient.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                users = await Json.ToObjectAsync<List<UserDto>>(content);
            }
            return users;
        }
    }
}
