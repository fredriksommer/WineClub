using System.Collections.Generic;

namespace WineClub.Core.Dtos
{
    public class RegionDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public List<WineDto> Wines { get; set; }

        public override string ToString()
        {
            return RegionName;
        }
    }
}
