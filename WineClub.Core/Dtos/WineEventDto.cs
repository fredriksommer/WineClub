using System;
using System.Collections.Generic;

namespace WineClub.Core.Dtos
{
    public class WineEventDto
    {
        public int WineEventId { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateAndTime { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public int MaxPersons { get; set; }
        public string Description { get; set; }

        public List<WineDto> Wines { get; set; }
        public List<UserDto> Attendees { get; set; }
    }
}
