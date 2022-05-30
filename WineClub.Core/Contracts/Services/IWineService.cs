using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IWineService
    {
        Task<IEnumerable<WineDto>> GetWinesAsync();
        Task<IEnumerable<WineDto>> GetTop3WinesAsync();
        Task<IEnumerable<WineDto>> GetWinesRatedByUserIdAsync(int userId);
        Task<WineDto> CreateWineAsync(WineDto wine);
        Task<bool> DeleteWineAsync(WineDto wine);
        Task<bool> EditWineAsync(WineDto wine);
        Task<WineDto> AddWineGrape(WineDto wine, GrapeDto grape);
        Task<bool> DeleteWineGrape(WineDto wine, GrapeDto grape);
        Task<WineDto> AddWineRegion(WineDto wine, RegionDto region);
        Task<bool> DeleteWineRegion(WineDto wine, RegionDto region);
        Task<WineDto> AddWineRating(WineDto wine, RatingDto rating);
    }
}
