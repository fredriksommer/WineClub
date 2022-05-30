using System.Collections.Generic;
using WineClub.Models;

namespace WineClub.Core.Dtos
{
    public class WineDto
    {
        public int WineId { get; set; }
        public string Name { get; set; }
        public WineryDto Winery { get; set; }
        public byte[] Image { get; set; }
        public double AlcoholContent { get; set; }
        public WineTypes WineType { get; set; }
        public int Year { get; set; }
        public UserDto AddedBy { get; set; }

        public List<GrapeDto> Grapes { get; set; }
        public List<RegionDto> Regions { get; set; }
        public List<RatingDto> Ratings { get; set; }
        public List<WineEventDto> Events { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
