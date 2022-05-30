using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using WineClub.Core.Constants;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Services;

namespace WineClub.ViewModels
{
    /// <summary>
    /// ViewModel for Wine Events.
    /// </summary>
    public class AddEditEventViewModel : ObservableValidator
    {
        private readonly WineEventDto _wineEventDto;
        private readonly UserLoggedInService _userLoggedInService;
        private readonly IWineService _wineService;

        /// <summary>
        /// Contructor when creating a new wine event
        /// </summary>
        public AddEditEventViewModel()
        {
            _wineEventDto = new();
            _userLoggedInService = Ioc.Default.GetService<UserLoggedInService>();
            _wineService = Ioc.Default.GetService<IWineService>();
            _ = LoadWines();
        }

        /// <summary>
        /// Constructor using parameter when using existing WineEvent
        /// </summary>
        /// <param name="wineEventdto"></param>
        public AddEditEventViewModel(WineEventDto wineEventdto)
        {
            _wineEventDto = wineEventdto;
            _userLoggedInService = Ioc.Default.GetService<UserLoggedInService>();
            _wineService = Ioc.Default.GetService<IWineService>();
            _ = LoadWines();
            NeededForUpdate();
        }

        /// <summary>
        /// To avoid bug using date and timepicker when editing Wine Event
        /// </summary>
        private void NeededForUpdate()
        {
            DateTimeOffset date = _wineEventDto == null ? DateTimeOffset.Now : _wineEventDto.DateAndTime;
            Date = date.Date;
            Time = date.DateTime.TimeOfDay;
            AddedWines = new ObservableCollection<WineDto>(Wines);
        }

        /// <summary>
        ///  Minimum Date for datepicker
        /// </summary>
        public DateTimeOffset MinDate => DateTimeOffset.Now;

        /// <summary>
        /// List of available Wines, used in create/edit wine event.
        /// </summary>
        private readonly List<WineDto> AvailableWines = new();
        public ObservableCollection<WineDto> AddedWines { get; private set; } = new();
        public List<WineDto> DeletedWines { get; private set; } = new();

        // Validation / Error handling
        private readonly List<ValidationResult> _errors = new();
        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in _errors select e.ErrorMessage);
        public bool ErrorsHaveI => Errors.Length > 0;

        public static explicit operator WineEventDto(AddEditEventViewModel e) => new()
        {
            WineEventId = e.WineEventId,
            Title = e.Title,
            DateAndTime = e.DateTime,
            City = e.City,
            StreetAddress = e.StreetAddress,
            MaxPersons = e.MaxPersons,
            Description = e.Description,
            Wines = e.Wines,
            Attendees = e.Attendees
        };

        /// <summary>
        /// Get or sets id of wine event
        /// </summary>
        public int WineEventId
        {
            get => _wineEventDto.WineEventId;
            set => SetProperty(_wineEventDto.WineEventId, value, _wineEventDto, (_wineEventDto, id) => _wineEventDto.WineEventId = value);
        }

        /// <summary>
        /// Gets or sets title of wine event.
        /// </summary>
        [Required(ErrorMessage = "Title is Required")]
        [MinLength(4, ErrorMessage = "Title should be longer than three characters")]
        public string Title
        {
            get => _wineEventDto.Title;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Title)));

                TrySetProperty(_wineEventDto.Title, value, (title) => _wineEventDto.Title = title, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets description of wine event.
        /// </summary>
        [Required(ErrorMessage = "Description is Required")]
        public string Description
        {
            get => _wineEventDto.Description;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Description)));

                TrySetProperty(_wineEventDto.Description, value, (description) => _wineEventDto.Description = description, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets location city for wine event.
        /// </summary>
        [Required(ErrorMessage = "City is Required")]
        public string City
        {
            get => _wineEventDto.City;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(City)));

                TrySetProperty(_wineEventDto.City, value, (city) => _wineEventDto.City = city, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets street address for wine event.
        /// </summary>
        [Required(ErrorMessage = "Street Address is Required")]
        public string StreetAddress
        {
            get => _wineEventDto.StreetAddress;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(StreetAddress)));

                TrySetProperty(_wineEventDto.StreetAddress, value, (streetAddress) => _wineEventDto.StreetAddress = streetAddress, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets max person of wine event.
        /// </summary>
        [Required(ErrorMessage = "Max Person is Required")]
        public int MaxPersons
        {
            get => _wineEventDto.MaxPersons;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(MaxPersons)));

                TrySetProperty(_wineEventDto.MaxPersons, value, (maxPersons) => _wineEventDto.MaxPersons = maxPersons, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Get or sets date of wine event.
        /// </summary>
        private DateTimeOffset _date;
        [Required(ErrorMessage = "Date is Required")]
        public DateTimeOffset Date
        {
            get => _date;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Date)));

                TrySetProperty(ref _date, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                DateTime = new(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds, new TimeSpan(2, 0, 0));
                OnPropertyChanged(nameof(DateTime));
            }
        }

        /// <summary>
        /// Gets or sets time of wine event.
        /// </summary>
        private TimeSpan _time;
        [Required(ErrorMessage = "Time is Required")]
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Time)));

                TrySetProperty(ref _time, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                DateTime = new(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds, new TimeSpan(2, 0, 0));
                OnPropertyChanged(nameof(DateTime));
            }
        }

        /// <summary>
        /// Gets or sets Date and Time of wine event.
        /// </summary>
        public DateTimeOffset DateTime
        {
            get => _wineEventDto.DateAndTime;
            set => SetProperty(_wineEventDto.DateAndTime, value, _wineEventDto, (_wineEventDto, dateAndTime) => _wineEventDto.DateAndTime = value);
        }

        /// <summary>
        /// Gets or sets wines in wine event.
        /// </summary>
        public List<WineDto> Wines
        {
            get => _wineEventDto.Wines ??= new();
            set => SetProperty(_wineEventDto.Wines, value, _wineEventDto, (_wineEvent, wines) => _wineEventDto.Wines = value);
        }

        /// <summary>
        /// Gets or sets attendees/participants of wine event.
        /// </summary>
        public List<UserDto> Attendees
        {
            get => _wineEventDto.Attendees ??= new();
            set => SetProperty(_wineEventDto.Attendees, value, _wineEventDto, (_wineEvent, attendees) => _wineEventDto.Attendees = value);
        }

        public WineDto SelectedWine { get; set; }

        private WineDto _selectedListWine;
        /// <summary>
        /// Gets or sets selected wine from list view.
        /// </summary>
        public WineDto SelectedListWine
        {
            get => _selectedListWine;
            set => SetProperty(ref _selectedListWine, value);
        }

        /// <summary>
        /// Event handler when text changes for auto suggest on wines, list filters as text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxWine_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<WineDto> suitableItems = new();
                string[] splitText = sender.Text.ToLower().Split(" ");
                foreach (WineDto item in AvailableWines)
                {
                    bool found = splitText.All((key) =>
                    {
                        return item.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                sender.ItemsSource = suitableItems;
            }
        }

        /// <summary>
        /// Chosen wine from autosuggest list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxWine_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SelectedWine = (WineDto)args.SelectedItem;
        }

        private ICommand _addWineCommand;
        /// <summary>
        /// Add Wine to Event Command.
        /// </summary>
        public ICommand AddWineCommand
        {
            get
            {
                if (_addWineCommand == null)
                {
                    _addWineCommand = new RelayCommand(() =>
                    {
                        if (SelectedWine != null)
                        {
                            AddedWines.Add(SelectedWine);
                            Wines = AddedWines.ToList();
                        }
                    });
                }
                return _addWineCommand;
            }
        }

        private ICommand _removeWineCommand;
        /// <summary>
        /// Remove wine from Event Command.
        /// </summary>
        public ICommand RemoveWineCommand
        {
            get
            {
                if (_removeWineCommand == null)
                {
                    _removeWineCommand = new RelayCommand(() =>
                    {
                        if (SelectedListWine != null)
                        {
                            DeletedWines.Add(SelectedListWine);
                            AddedWines.Remove(SelectedListWine);
                            Wines = AddedWines.ToList();

                        }
                    });
                }
                return _removeWineCommand;
            }
        }

        public int Participants => _wineEventDto.Attendees != null ? _wineEventDto.Attendees.Count : 0;

        /// <summary>
        /// Loading wines to use in add/edit wine event.
        /// </summary>
        /// <returns></returns>
        private async Task LoadWines()
        {
            try
            {
                IEnumerable<WineDto> wines = await _wineService.GetWinesAsync();
                foreach (WineDto wine in wines)
                {
                    AvailableWines.Add(wine);
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Error");
            }
        }

        /// <summary>
        /// BitMapImage using Google Maps Static API.
        /// </summary>
        public BitmapImage MapImage => new(new Uri(BaseAddress.MapUrl(City, StreetAddress)));


    }
}
