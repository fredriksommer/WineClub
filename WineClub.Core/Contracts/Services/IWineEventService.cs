using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IWineEventService
    {
        Task<IEnumerable<WineEventDto>> GetWineEventsAsync();
        Task<IEnumerable<WineEventDto>> GetNextWineEventAsync();
        Task<WineEventDto> CreateWineEventAsync(WineEventDto wineEvent);
        Task<bool> DeleteWineEventAsync(WineEventDto wineEvent);
        Task<bool> EditWineEventAsync(WineEventDto wineEvent);
        Task<WineEventDto> AddWineEventWine(WineEventDto wineEvent, WineDto wine);
        Task<bool> DeleteWineEventWine(WineEventDto wineEvent, WineDto wine);
        Task<WineEventDto> AddWineEventUser(WineEventDto wineEvent, UserDto user);
        Task<bool> DeleteWineEventUser(WineEventDto wineEvent, UserDto user);
    }
}
