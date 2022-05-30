
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Contracts.ViewModels;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Services;

namespace WineClub.ViewModels
{
    public class ProfileViewModel : ObservableRecipient, INavigationAware
    {
        private readonly UserLoggedInService _userLoggedInService;
        private readonly INavigationService _navigationService;
        private readonly IWineService _wineService;
        private readonly IWineEventService _wineEventService;

        public ProfileViewModel(UserLoggedInService userLoggedInService, INavigationService navigationService, IWineService wineService, IWineEventService wineEventService)
        {
            _userLoggedInService = userLoggedInService;
            _navigationService = navigationService;
            _wineService = wineService;
            _wineEventService = wineEventService;
        }

        public UserDto User => _userLoggedInService.User;

        public AddEditEventViewModel NextEvent { get; set; }
        public ObservableCollection<AddWineViewModel> MyReviews { get; private set; } = new();

        public bool NextEventVisibilty => NextEvent != null;
        public bool MyReviewsVisibilty => MyReviews.Count > 0;

        public void ContentGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            _userLoggedInService.WineButtonMainPage = e.ClickedItem as AddWineViewModel;
            _navigationService.NavigateTo(typeof(WineListViewModel).FullName);
        }

        private ICommand _goToEventCommand;
        public ICommand ToToEventCommand
        {
            get
            {
                if (_goToEventCommand == null)
                {
                    _goToEventCommand = new RelayCommand(() =>
                    {
                        _userLoggedInService.EventsGoToPage = NextEvent;
                        _navigationService.NavigateTo(typeof(EventDetailedViewModel).FullName);
                    }
                    );
                }
                return _goToEventCommand;
            }
        }

        private ICommand _editProfiletCommand;
        public ICommand EditProfileCommand
        {
            get
            {
                if (_editProfiletCommand == null)
                {
                    _editProfiletCommand = new RelayCommand(async () =>
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "Edit Profile",
                            Content = "Not yet implemented",
                            CloseButtonText = "Ok",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        ContentDialogResult result = await dialog.ShowAsync();
                    }
                    );
                }
                return _editProfiletCommand;
            }
        }

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo(object parameter)
        {
            IEnumerable<WineDto> winesRated = await _wineService.GetWinesRatedByUserIdAsync(User.UserId);

            foreach (WineDto wine in winesRated)
            {
                MyReviews.Add(new(wine));
            }
            OnPropertyChanged(nameof(MyReviewsVisibilty));

            IEnumerable<WineEventDto> wineEvents = await _wineEventService.GetWineEventsAsync();
            WineEventDto nextEvent = wineEvents.Where(x => x.DateAndTime > System.DateTimeOffset.Now).FirstOrDefault(x => x.Attendees.Any(u => u.UserId == User.UserId));

            if (nextEvent != null)
            {
                NextEvent = new(nextEvent);
                OnPropertyChanged(nameof(NextEvent));
            }
            OnPropertyChanged(nameof(NextEventVisibilty));
        }
    }
}
