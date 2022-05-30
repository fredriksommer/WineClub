using System.Threading.Tasks;
using WineClub.Core.Dtos;

namespace WineClub.Core.Contracts.Services
{
    public interface IRatingService
    {
        Task<RatingDto> CreateRatingAsync(RatingDto rating);
    }
}
