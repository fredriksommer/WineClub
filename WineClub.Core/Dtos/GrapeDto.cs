using System.Collections.Generic;

namespace WineClub.Core.Dtos
{
    public class GrapeDto
    {
        public int GrapeId { get; set; }
        public string GrapeName { get; set; }

        public List<WineDto> Wines { get; set; }

        public override string ToString()
        {
            return GrapeName;
        }
    }
}
