using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IWineryService
    {
        Task<IEnumerable<WineryDto>> GetWineriesAsync();
        Task<WineryDto> CreateWineryAsync(WineryDto wine);
        Task<bool> DeleteWineryAsync(WineryDto wine);
        Task<bool> EditWineryAsync(WineryDto wine);
    }
}
