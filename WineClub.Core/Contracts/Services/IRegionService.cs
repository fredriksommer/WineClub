using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetRegionsAsync();
        Task<RegionDto> CreateRegionAsync(RegionDto region);
        Task<bool> DeleteRegionAsync(RegionDto regionDto);
        Task<bool> EditRegionAsync(RegionDto regionDto);
    }
}
