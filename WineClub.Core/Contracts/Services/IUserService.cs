using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> CreateUserAsync(UserDto user);
        Task<bool> DeleteUserAsync(UserDto user);
        Task<bool> EditUserAsync(UserDto user);
    }
}
