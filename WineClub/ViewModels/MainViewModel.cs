using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Contracts.ViewModels;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Services;

namespace WineClub.ViewModels
{
    public class MainViewModel : ObservableRecipient, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IWineService _wineService;
        private readonly IWineEventService _wineEventService;
        private readonly UserLoggedInService _userLoggedInService;

        /// <summary>
        /// Constructor to create MainViewModel
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="wineService"></param>
        /// <param name="userLoggedInService"></param>
        /// <param name="wineEventService"></param>
        public MainViewModel(INavigationService navigationService, IWineService wineService, UserLoggedInService userLoggedInService, IWineEventService wineEventService)
        {
            _navigationService = navigationService;
            _wineService = wineService;
            _wineEventService = wineEventService;
            _userLoggedInService = userLoggedInService;
        }

        /// <summary>
        /// List of Top 3 wines order by Rating
        /// </summary>
        public ObservableCollection<AddWineViewModel> Wines { get; private set; } = new ObservableCollection<AddWineViewModel>();

        /// <summary>
        /// Next event by date.
        /// </summary>
        public AddEditEventViewModel NextEvent { get; private set; }

        private ICommand _goToEventCommand;

        /// <summary>
        /// Go to event Command.
        /// </summary>
        public ICommand ToToEventCommand
        {
            get
            {
                if (_goToEventCommand == null)
                {
                    _goToEventCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo(typeof(EventDetailedViewModel).FullName);
                    }
                    );
                }
                return _goToEventCommand;
            }

        }

        private ICommand _attendEventCommand;

        /// <summary>
        /// Command to attend event on mainpage.
        /// </summary>
        public ICommand AttendEventCommand
        {
            get
            {
                if (_attendEventCommand == null)
                {
                    _attendEventCommand = new RelayCommand(async () =>
                    {
                        if (_userLoggedInService.User != null)
                        {
                            if (NextEvent.Participants < NextEvent.MaxPersons)
                            {
                                WineEventDto wineEvent = await _wineEventService.AddWineEventUser((WineEventDto)NextEvent, _userLoggedInService.User);
                                ContentDialog dialog = new()
                                {
                                    Title = "Thank you!",
                                    Content = "See you there. We're glad you are attending the event.",
                                    PrimaryButtonText = "Ok",
                                    DefaultButton = ContentDialogButton.Primary,
                                    XamlRoot = _navigationService.Frame.XamlRoot,

                                };

                                ContentDialogResult result = await dialog.ShowAsync();
                                NextEvent = new AddEditEventViewModel(wineEvent);
                                OnPropertyChanged(nameof(NextEvent));
                                AttendingVisibilty = false;
                                OnPropertyChanged(nameof(AttendingVisibilty));
                            }
                        }
                    }
                    );
                }
                return _attendEventCommand;
            }
        }

        /// <summary>
        /// Gets boolean to check if NextEvent is not equal to null. For visibilty purpose.
        /// </summary>
        public bool NextEventVisibility => NextEvent != null;

        /// <summary>
        /// Gets boolean to check if attend button is enable or not.
        /// </summary>
        public bool AttendButtonEnable => _userLoggedInService.IsLoggedIn;

        /// <summary>
        /// Gets visibilty for attend button.
        /// </summary>
        public bool AttendingVisibilty { get; set; }

        /// <summary>
        /// Gets visibilty for stop attend button.
        /// </summary>
        public bool StopAttendVisibilty { get; set; }

        /// <summary>
        /// Navigates to selected wine from grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContentGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            _userLoggedInService.WineButtonMainPage = e.ClickedItem as AddWineViewModel;
            _navigationService.NavigateTo(typeof(WineListViewModel).FullName);
        }


        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// When navigated to, logic to set top 3 wines including next event.
        /// </summary>
        /// <param name="parameter"></param>
        public async void OnNavigatedTo(object parameter)
        {
            if (Wines.Count == 0)
            {
                try
                {
                    IEnumerable<WineDto> wines = await _wineService.GetTop3WinesAsync();
                    List<AddWineViewModel> viewModelList = new();

                    if (wines.Count() > 0)
                    {
                        foreach (WineDto wine in wines)
                        {
                            Wines.Add(new AddWineViewModel(wine));
                        }
                    }

                    IEnumerable<WineEventDto> events = await _wineEventService.GetNextWineEventAsync();

                    if (events.Count() > 0)
                    {
                        NextEvent = new(events.FirstOrDefault());
                        List<UserDto> attendees = events.FirstOrDefault().Attendees;
                        OnPropertyChanged(nameof(NextEvent));
                        OnPropertyChanged(nameof(NextEventVisibility));
                        _userLoggedInService.EventsGoToPage = NextEvent;


                        if (_userLoggedInService.IsLoggedIn)
                        {
                            UserDto attending = attendees.FirstOrDefault(x => x.UserId == _userLoggedInService.User.UserId);
                            if (attending == null)
                            {
                                AttendingVisibilty = true;
                                OnPropertyChanged(nameof(AttendingVisibilty));
                            }
                            else
                            {
                                StopAttendVisibilty = true;
                                OnPropertyChanged(nameof(StopAttendVisibilty));
                            }
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    ContentDialog dialog = new()
                    {
                        Title = "Could not connect to API",
                        Content = "Make sure you have internet before using this app",
                        PrimaryButtonText = "Ok",
                        DefaultButton = ContentDialogButton.Primary,
                        XamlRoot = _navigationService.Frame.XamlRoot,

                    };

                    ContentDialogResult result = await dialog.ShowAsync();
                }

            }
        }
    }
}
