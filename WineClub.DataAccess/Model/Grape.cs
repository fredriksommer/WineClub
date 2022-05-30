using System.Collections.Generic;

namespace WineClub.DataAccess.Model
{
    public class Grape
    {
        public int GrapeId { get; set; }
        public string GrapeName { get; set; }

        public List<Wine> Wines { get; set; }
    }
}