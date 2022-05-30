using System;
using System.Collections.Generic;

namespace WineClub.DataAccess.Model
{
    public class WineEvent
    {
        public int WineEventId { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateAndTime { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public int MaxPersons { get; set; }
        public string Description { get; set; }

        public List<Wine> Wines { get; set; }
        public List<User> Attendees { get; set; }
    }
}
