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
using WineClub.Views;

namespace WineClub.ViewModels
{
    public class EventsViewModel : ObservableRecipient, INavigationAware
    {
        private readonly UserLoggedInService _userLoggedInService;
        private readonly INavigationService _navigationService;
        private readonly IWineEventService _wineEventService;

        public EventsViewModel(INavigationService navigationService, UserLoggedInService userLoggedInService, IWineEventService wineEventService)
        {
            _navigationService = navigationService;
            _userLoggedInService = userLoggedInService;
            _wineEventService = wineEventService;
        }

        public ObservableCollection<AddEditEventViewModel> UpcomingEvents { get; private set; } = new ObservableCollection<AddEditEventViewModel>();

        /// <summary>
        /// Boolean to check if user has admin role or not.
        /// </summary>
        public bool AdminRole => _userLoggedInService.UserRole == Models.UserRoles.Admin;

        private ICommand _addEventCommand;
        /// <summary>
        /// Command to add event, also adds many to many wines.
        /// </summary>
        public ICommand AddEventCommand
        {
            get
            {
                if (_addEventCommand == null)
                {
                    _addEventCommand = new RelayCommand(async () =>
                    {
                        AddEditEventViewModel addEvent = new();
                        AddEditEventPage page = new(addEvent);

                        ContentDialog dialog = new()
                        {
                            Title = "Add New Wine Event",
                            Content = page,
                            PrimaryButtonText = "Add Event",
                            CloseButtonText = "Cancel",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        addEvent.PropertyChanged += (sender, e) => dialog.IsPrimaryButtonEnabled = !addEvent.ErrorsHaveI && addEvent.Date.Year != 0;

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            WineEventDto eventToAdd = (WineEventDto)addEvent;

                            ObservableCollection<WineDto> wines = addEvent.AddedWines;
                            eventToAdd.Wines = null;

                            WineEventDto wineEventCreated = await _wineEventService.CreateWineEventAsync(eventToAdd);

                            foreach (WineDto wine in wines)
                            {
                                _ = await _wineEventService.AddWineEventWine(wineEventCreated, wine);
                            }

                            UpcomingEvents.Add(new AddEditEventViewModel(wineEventCreated));
                            _ = UpcomingEvents.OrderBy(x => x.DateTime);
                            OnPropertyChanged(nameof(UpcomingEvents));
                        }

                    });
                }
                return _addEventCommand;
            }
        }

        /// <summary>
        /// Method runs every time you get navigated to this page/viewmodel. 
        /// Login is required to get here.
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

            if (UpcomingEvents.Count == 0)
            {
                IEnumerable<WineEventDto> events = await _wineEventService.GetWineEventsAsync();
                IOrderedEnumerable<WineEventDto> query = events.Where(x => x.DateAndTime > DateTimeOffset.Now).OrderBy(x => x.DateAndTime);

                foreach (WineEventDto wineEvent in query)
                {
                    UpcomingEvents.Add(new AddEditEventViewModel(wineEvent));
                }

            }
        }

        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// When event is clicked in grid view, you get navigated to event detailed page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContentGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _userLoggedInService.EventsGoToPage = e.ClickedItem as AddEditEventViewModel;
            _navigationService.NavigateTo(typeof(EventDetailedViewModel).FullName);
        }
    }
}
