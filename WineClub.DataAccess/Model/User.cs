using System.Collections.Generic;
using WineClub.Models;

namespace WineClub.DataAccess.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }

        public List<Wine> Wines { get; set; }
        public List<WineEvent> WineEvents { get; set; }

    }
}
