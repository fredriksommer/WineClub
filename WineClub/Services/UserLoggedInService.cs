using CommunityToolkit.Mvvm.ComponentModel;
using WineClub.Core.Dtos;
using WineClub.Models;
using WineClub.ViewModels;

namespace WineClub.Services
{
    public class UserLoggedInService : ObservableRecipient
    {
        public UserDto User { get; set; }

        public AddWineViewModel WineButtonMainPage { get; set; }

        public AddEditEventViewModel EventsGoToPage { get; set; }

        public bool HideVisibility => !IsLoggedIn;

        public bool IsLoggedIn => User != null;

        public UserRoles? UserRole => User?.Role;

        public void LogIn(UserDto user)
        {
            User = user;
            OnPropertyChanged(nameof(HideVisibility));
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        public void LogOut()
        {
            User = null;
            OnPropertyChanged(nameof(HideVisibility));
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }
}
