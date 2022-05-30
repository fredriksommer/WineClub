using System.Collections.Generic;
using WineClub.Models;

namespace WineClub.Core.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }

        public List<WineDto> Wines { get; set; }
        public List<WineEventDto> WineEvents { get; set; }
    }
}
