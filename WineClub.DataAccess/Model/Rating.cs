using System;
using System.Collections.Generic;

namespace WineClub.DataAccess.Model
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Score { get; set; }
        public string ReviewText { get; set; }

        public List<Wine> Wines { get; set; }
        public User RatedBy { get; set; }
        public DateTimeOffset DateOfRating { get; set; }
    }
}
