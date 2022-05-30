using System.Collections.Generic;
using WineClub.Models;

namespace WineClub.DataAccess.Model
{
    public class Wine
    {
        public int WineId { get; set; }
        public string Name { get; set; }
        public Winery Winery { get; set; }
        public byte[] Image { get; set; }
        public double AlcoholContent { get; set; }
        public WineTypes WineType { get; set; }
        public int Year { get; set; }
        public User AddedBy { get; set; }

        public List<Grape> Grapes { get; set; }
        public List<Region> Regions { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<WineEvent> Events { get; set; }

    }
}
