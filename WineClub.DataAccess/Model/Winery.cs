using System.Collections.Generic;

namespace WineClub.DataAccess.Model
{
    public class Winery
    {
        public int WineryId { get; set; }
        public string Name { get; set; }

        public List<Wine> Wines { get; set; }
    }
}