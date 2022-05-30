using System.Collections.Generic;

namespace WineClub.Core.Dtos
{
    public class WineryDto
    {
        public int WineryId { get; set; }
        public string Name { get; set; }

        public List<WineDto> Wines { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
