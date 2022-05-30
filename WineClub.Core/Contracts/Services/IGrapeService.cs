using System.Collections.Generic;
using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IGrapeService
    {
        Task<IEnumerable<GrapeDto>> GetGrapesAsync();
        Task<GrapeDto> CreateGrapeAsync(GrapeDto grape);
        Task<bool> DeleteGrapeAsync(GrapeDto grape);
        Task<bool> EditGrapeAsync(GrapeDto grape);
    }
}
