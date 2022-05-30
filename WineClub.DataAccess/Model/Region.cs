using System.Collections.Generic;

namespace WineClub.DataAccess.Model
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public List<Wine> Wines { get; set; }
    }
}