
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Contracts.ViewModels;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Services;
using WineClub.Views;

namespace WineClub.ViewModels
{
    /// <summary>
    /// Detailed event ViewModel. Used to get a more detailed view over events, with button to attend/stop-attend, and edit/delete for admins.
    /// </summary>
    public class EventDetailedViewModel : ObservableRecipient, INavigationAware
    {
        private readonly UserLoggedInService _userLoggedInService;
        private readonly INavigationService _navigationService;
        private readonly IWineEventService _wineEventService;

        /// <summary>
        /// Conctructor to create a new ViewModel
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="userLoggedInService"></param>
        /// <param name="wineEventService"></param>
        public EventDetailedViewModel(INavigationService navigationService, UserLoggedInService userLoggedInService, IWineEventService wineEventService)
        {
            _navigationService = navigationService;
            _userLoggedInService = userLoggedInService;
            _wineEventService = wineEventService;
            WineEvent = _userLoggedInService.EventsGoToPage;
        }

        /// <summary>
        /// AddEditEventViewModel is used to get data to page.
        /// </summary>
        public AddEditEventViewModel WineEvent { get; set; }

        /// <summary>
        /// List of wines for wine event.
        /// </summary>
        public ObservableCollection<AddWineViewModel> Wines { get; private set; } = new();

        /// <summary>
        /// Boolean to check wether the user have admin role or not.
        /// </summary>
        public bool AdminRole => _userLoggedInService.UserRole == Models.UserRoles.Admin;

        /// <summary>
        /// Boolean for visibilty for attend button if user is logged in.
        /// </summary>
        public bool AttendButtonEnable => _userLoggedInService.IsLoggedIn;

        /// <summary>
        /// Boolean for attend button visibilty if user not alredy are attending event.
        /// </summary>
        public bool AttendingVisibilty { get; set; }

        /// <summary>
        /// Boolean for stop attend button visibilty if user already are attending event.
        /// </summary>
        public bool StopAttendVisibilty { get; set; }

        /// <summary>
        /// When user click on grid view for wines.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContentGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            _userLoggedInService.WineButtonMainPage = e.ClickedItem as AddWineViewModel;
            _navigationService.NavigateTo(typeof(WineListViewModel).FullName);
        }

        private ICommand _editEventCommand;
        /// <summary>
        /// Command to edit wine events, including many to many relationships, only admin.
        /// </summary>
        public ICommand EditEventCommand
        {
            get
            {
                if (_editEventCommand == null)
                {
                    _editEventCommand = new RelayCommand(async () =>
                    {
                        AddEditEventViewModel editEvent = WineEvent;
                        AddEditEventPage page = new(editEvent);

                        ContentDialog dialog = new()
                        {
                            Title = "Edit Wine Event",
                            Content = page,
                            PrimaryButtonText = "Edit Event",
                            CloseButtonText = "Cancel",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        editEvent.PropertyChanged += (sender, e)
                        => dialog.IsPrimaryButtonEnabled = !editEvent.ErrorsHaveI;

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            WineEventDto wineEventToEdit = (WineEventDto)editEvent;
                            ObservableCollection<WineDto> wines = editEvent.AddedWines;
                            System.Collections.Generic.List<WineDto> winesAdded = wineEventToEdit.Wines;
                            wineEventToEdit.Wines = null;
                            System.Collections.Generic.List<WineDto> deletedWines = editEvent.DeletedWines;

                            // Delete wine(s)
                            foreach (WineDto wine in deletedWines)
                            {
                                if (!wines.Contains(wine))
                                {
                                    _ = await _wineEventService.DeleteWineEventWine(wineEventToEdit, wine);
                                    Wines.Remove(Wines.FirstOrDefault(x => x.WineId == wine.WineId));

                                }
                            }

                            // Add wine(s)
                            foreach (WineDto wine in wines)
                            {
                                if (!Wines.Any(w => w.WineId == wine.WineId))
                                {
                                    _ = await _wineEventService.AddWineEventWine(wineEventToEdit, wine);
                                    Wines.Add(new AddWineViewModel(wine));
                                }
                            }

                            if (await _wineEventService.EditWineEventAsync(wineEventToEdit))
                            {
                                WineEvent = editEvent;
                                OnPropertyChanged(nameof(WineEvent));
                            }
                        }
                    });
                }
                return _editEventCommand;
            }
        }

        private ICommand _deleteEventCommand;
        /// <summary>
        /// Command to delete event, only admin.
        /// </summary>
        public ICommand DeleteEventCommand
        {
            get
            {
                if (_deleteEventCommand == null)
                {
                    _deleteEventCommand = new RelayCommand(async () =>
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "Delete Wine Event",
                            Content = "If you delete this wine event, you won't be able to recover it. Do you want to delete it?",
                            PrimaryButtonText = "Delete Event",
                            CloseButtonText = "Cancel",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };
                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            if (await _wineEventService.DeleteWineEventAsync((WineEventDto)WineEvent))
                            {
                                _navigationService.NavigateTo(typeof(EventsViewModel).FullName);
                            }
                        }
                    });
                }
                return _deleteEventCommand;
            }
        }

        private ICommand _attendEventCommand;
        /// <summary>
        /// Command for users to attend wine events.
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
                            if (WineEvent.Participants < WineEvent.MaxPersons)
                            {
                                WineEventDto wineEvent = await _wineEventService.AddWineEventUser((WineEventDto)WineEvent, _userLoggedInService.User);
                                ContentDialog dialog = new()
                                {
                                    Title = "Thank you!",
                                    Content = "See you there. We're glad you are attending the event.",
                                    PrimaryButtonText = "Ok",
                                    DefaultButton = ContentDialogButton.Primary,
                                    XamlRoot = _navigationService.Frame.XamlRoot,

                                };

                                ContentDialogResult result = await dialog.ShowAsync();
                                AttendingVisibilty = false;
                                StopAttendVisibilty = true;
                                WineEvent = new AddEditEventViewModel(wineEvent);
                                OnPropertyChanged(nameof(WineEvent));
                                OnPropertyChanged(nameof(AttendingVisibilty));
                                OnPropertyChanged(nameof(StopAttendVisibilty));
                            }
                            else
                            {
                                ContentDialog dialog = new()
                                {
                                    Title = "Fail",
                                    Content = "Event is full.",
                                    PrimaryButtonText = "Ok",
                                    DefaultButton = ContentDialogButton.Primary,
                                    XamlRoot = _navigationService.Frame.XamlRoot,

                                };

                                ContentDialogResult result = await dialog.ShowAsync();
                            }
                        }
                    }
                    );
                }
                return _attendEventCommand;
            }
        }

        private ICommand _stopAttendEventCommand;
        /// <summary>
        /// Command to where users can cancel/stop attending for wine events.
        /// </summary>
        public ICommand StopAttendEventCommand
        {
            get
            {
                if (_stopAttendEventCommand == null)
                {
                    _stopAttendEventCommand = new RelayCommand(async () =>
                    {
                        if (_userLoggedInService.User != null)
                        {
                            if (await _wineEventService.DeleteWineEventUser((WineEventDto)WineEvent, _userLoggedInService.User))
                            {
                                ContentDialog dialog = new()
                                {
                                    Title = "Success",
                                    Content = "You are no longer attending this event",
                                    PrimaryButtonText = "Ok",
                                    DefaultButton = ContentDialogButton.Primary,
                                    XamlRoot = _navigationService.Frame.XamlRoot,

                                };

                                ContentDialogResult result = await dialog.ShowAsync();
                                AttendingVisibilty = true;
                                StopAttendVisibilty = false;
                                OnPropertyChanged(nameof(AttendingVisibilty));
                                OnPropertyChanged(nameof(StopAttendVisibilty));
                            }
                        }
                    }
                    );
                }
                return _stopAttendEventCommand;
            }
        }

        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// Method runs every time you get navigated to this page. Add wines to Wines collection and fixes visibilty for attend/stop-attend buttons.
        /// </summary>
        /// <param name="parameter"></param>
        public async void OnNavigatedTo(object parameter)
        {
            if (!_userLoggedInService.IsLoggedIn)
            {


                ContentDialog dialog = new()
                {
                    Title = "Please login",
                    Content = "In order to use this app, you need to login",
                    PrimaryButtonText = "Login",
                    SecondaryButtonText = "Sign Up",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = _navigationService.Frame.XamlRoot
                };

                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    _navigationService.NavigateTo(typeof(LoginViewModel).FullName);
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    _navigationService.NavigateTo(typeof(SignUpViewModel).FullName);
                }

            }

            foreach (WineDto wine in WineEvent.Wines)
            {
                Wines.Add(new AddWineViewModel(wine));
            }

            if (_userLoggedInService.IsLoggedIn)
            {
                UserDto attending = WineEvent.Attendees.FirstOrDefault(x => x.UserId == _userLoggedInService.User.UserId);
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
}
