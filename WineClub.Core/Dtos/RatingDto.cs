using System;
using System.Collections.Generic;

namespace WineClub.Core.Dtos
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public int Score { get; set; }
        public string ReviewText { get; set; }

        public List<WineDto> Wines { get; set; }
        public UserDto RatedBy { get; set; }
        public DateTimeOffset DateOfRating { get; set; }
    }
}
