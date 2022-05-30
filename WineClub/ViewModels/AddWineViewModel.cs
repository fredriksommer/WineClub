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
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WineClub.Contracts.Services;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Models;
using WineClub.Services;

namespace WineClub.ViewModels
{
    /// <summary>
    /// ViewModel for wine, also used in add/edit wine.
    /// </summary>
    public class AddWineViewModel : ObservableValidator
    {
        private readonly WineDto _wineDto;
        private readonly RatingDto _ratingDto;
        private readonly IWineryService _wineryService;
        private readonly IWineService _wineService;
        private readonly IRegionService _regionService;
        private readonly IGrapeService _grapeService;
        private readonly IRatingService _ratingService;
        private readonly UserLoggedInService _userLoggedInService;
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Creates a new AddWineViewModel, creates new wineDto
        /// </summary>
        public AddWineViewModel()
        {
            _wineDto = new WineDto();
            _ratingDto = new RatingDto();
            _wineryService = Ioc.Default.GetService<IWineryService>();
            _wineService = Ioc.Default.GetService<IWineService>();
            _regionService = Ioc.Default.GetService<IRegionService>();
            _grapeService = Ioc.Default.GetService<IGrapeService>();
            _ratingService = Ioc.Default.GetService<IRatingService>();
            _navigationService = Ioc.Default.GetService<INavigationService>();
            _userLoggedInService = Ioc.Default.GetService<UserLoggedInService>();
            LoadData();
        }

        /// <summary>
        /// Creates a new AddWineViewModel depending on parameter wine (WineDto).
        /// </summary>
        public AddWineViewModel(WineDto wine)
        {
            _wineDto = wine;
            _ratingDto = new RatingDto();
            _wineryService = Ioc.Default.GetService<IWineryService>();
            _wineService = Ioc.Default.GetService<IWineService>();
            _regionService = Ioc.Default.GetService<IRegionService>();
            _grapeService = Ioc.Default.GetService<IGrapeService>();
            _ratingService = Ioc.Default.GetService<IRatingService>();
            _navigationService = Ioc.Default.GetService<INavigationService>();
            _userLoggedInService = Ioc.Default.GetService<UserLoggedInService>();
            LoadData();
            LoadImage();
            GetCollectionIfEdit();
            LoadRatings();
        }


        private readonly List<WineryDto> Wineries = new();

        private readonly List<RegionDto> AvailableRegions = new();
        /// <summary>
        /// List to handle added regions when create/edit wine.
        /// </summary>
        public ObservableCollection<RegionDto> AddedRegions { get; private set; } = new();

        /// <summary>
        /// List to handle deleted regions when edit wine.
        /// </summary>
        public List<RegionDto> DeletedRegions { get; private set; } = new();
        private RegionDto newRegion;
        private RegionDto SelectedRegion { get; set; }

        /// <summary>
        /// List of all grapes available from db.
        /// </summary>
        private readonly List<GrapeDto> AvailableGrapes = new();

        /// <summary>
        /// List to handle added grapes when create/edit wine.
        /// </summary>
        public ObservableCollection<GrapeDto> AddedGrapes { get; private set; } = new();

        /// <summary>
        /// List to handle deleted grapes when edit wine.
        /// </summary>
        public List<GrapeDto> DeletedGrapes { get; private set; } = new();

        private GrapeDto newGrape;
        private GrapeDto SelectedGrape { get; set; }

        /// <summary>
        /// List of all ratings of wine.
        /// </summary>
        public ObservableCollection<RatingDto> RatingsList = new();

        /// <summary>
        /// Explicit operator to create WineDto object
        /// </summary>
        /// <param name="w"></param>
        public static explicit operator WineDto(AddWineViewModel w) => new()
        {
            WineId = w.WineId,
            Name = w.Name,
            Year = w.Year,
            WineType = w.SelectedWineType,
            Image = w.ByteImage,
            Winery = w.Winery,
            Regions = w.Regions,
            Grapes = w.Grapes,
        };

        private RegionDto _selectedListRegion;
        /// <summary>
        /// Selected region in list view for add/edit wine.
        /// </summary>
        public RegionDto SelectedListRegion
        {
            get => _selectedListRegion;
            set => SetProperty(ref _selectedListRegion, value);
        }

        private GrapeDto _selectedListGrape;
        /// <summary>
        /// Selected grape in list view for add/edit wine.
        /// </summary>
        public GrapeDto SelectedListGrape
        {
            get => _selectedListGrape;
            set => SetProperty(ref _selectedListGrape, value);
        }

        /// <summary>
        /// Load all data required to add/edit wines.
        /// </summary>
        private void LoadData()
        {
            LoadRegions();
            LoadWineries();
            LoadGrapes();
        }

        /// <summary>
        /// Sets grape and region collection correct when user want to edit.
        /// </summary>
        private void GetCollectionIfEdit()
        {
            AddedGrapes = new ObservableCollection<GrapeDto>(Grapes);
            AddedRegions = new ObservableCollection<RegionDto>(Regions);
        }

        /// <summary>
        /// Convert image from db to BitmapImage when constructor has viewmodel parameter.
        /// </summary>
        private void LoadImage()
        {
            if (_wineDto.Image != null)
            {
                GetBitmapImage(_wineDto.Image);
            }
        }

        /// <summary>
        /// Load Wineries from Rest-Api.
        /// </summary>
        private async void LoadWineries()
        {
            IEnumerable<WineryDto> data = await _wineryService.GetWineriesAsync();
            foreach (WineryDto item in data)
            {
                Wineries.Add(item);
            }
        }

        /// <summary>
        /// Load Regions from Rest-Api.
        /// </summary>
        private async void LoadRegions()
        {
            IEnumerable<RegionDto> data = await _regionService.GetRegionsAsync();
            foreach (RegionDto item in data)
            {
                AvailableRegions.Add(item);
            }
        }

        /// <summary>
        /// Load Grapes from Rest-Api.
        /// </summary>
        private async void LoadGrapes()
        {
            IEnumerable<GrapeDto> data = await _grapeService.GetGrapesAsync();
            foreach (GrapeDto item in data)
            {
                AvailableGrapes.Add(item);
            }
        }

        /// <summary>
        ///  EventHandler when winery text changes. Itemssource changes depending on text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxWinery_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<WineryDto> suitableItems = new();
                string[] splitText = sender.Text.ToLower().Split(" ");
                foreach (WineryDto item in Wineries)
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
                if (suitableItems.Count == 0)
                {
                    Winery = new WineryDto { Name = sender.Text };
                }
                sender.ItemsSource = suitableItems;
            }
        }

        /// <summary>
        /// AutoSuggest Chosen - sets Winery property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxWinery_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Winery = (WineryDto)args.SelectedItem;
        }

        /// <summary>
        ///  EventHandler when region text changes. Itemssource changes depending on text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxRegions_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<RegionDto> suitableItems = new();
                string[] splitText = sender.Text.ToLower().Split(" ");
                foreach (RegionDto item in AvailableRegions)
                {
                    bool found = splitText.All((key) =>
                    {
                        return item.RegionName.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    newRegion = new RegionDto() { RegionName = sender.Text };
                }
                sender.ItemsSource = suitableItems;
            }
        }

        /// <summary>
        /// Select region in listview when add/remove regions of wine, sets SelectedRegion to chosen region.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxRegions_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SelectedRegion = (RegionDto)args.SelectedItem;
        }

        /// <summary>
        /// EventHandler when grape text changes. Itemssource changes depending on text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxGrapes_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<GrapeDto> suitableItems = new();
                string[] splitText = sender.Text.ToLower().Split(" ");
                foreach (GrapeDto item in AvailableGrapes)
                {
                    bool found = splitText.All((key) =>
                    {
                        return item.GrapeName.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    newGrape = new GrapeDto() { GrapeName = sender.Text };
                }
                sender.ItemsSource = suitableItems;
            }
        }

        /// <summary>
        /// Select grape in listview when add/remove grapes of wine, sets SelectedGrape to chosen grape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxGrapes_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SelectedGrape = (GrapeDto)args.SelectedItem;
        }

        private readonly List<ValidationResult> _errors = new();
        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in _errors select e.ErrorMessage);
        public bool ErrorsHaveI => Errors.Length > 0;

        /// <summary>
        /// Gets or sets wineid on wine.
        /// </summary>
        public int WineId
        {
            get => _wineDto.WineId;
            set => SetProperty(_wineDto.WineId, value, _wineDto, (_wineDto, id) => _wineDto.WineId = value);
        }

        /// <summary>
        /// Gets or sets name of wine.
        /// </summary>
        [Required(ErrorMessage = "Name is Required")]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public string Name
        {
            get => _wineDto.Name;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Name)));

                TrySetProperty(_wineDto.Name, value, (name) => _wineDto.Name = name, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets year of wine.
        /// </summary>
        [Required(ErrorMessage = "Year is Required")]
        public int Year
        {
            get => _wineDto.Year;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Year)));

                TrySetProperty(_wineDto.Year, value, (year) => _wineDto.Year = year, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets byte[] (Image) of wine.
        /// </summary>
        public byte[] ByteImage
        {
            get => _wineDto.Image;
            set => SetProperty(_wineDto.Image, value, _wineDto, (_wineDto, byteImage) => _wineDto.Image = value);
        }

        /// <summary>
        /// Gets or sets wine type of wine.
        /// </summary>
        public WineTypes SelectedWineType
        {
            get => _wineDto.WineType;
            set => SetProperty(_wineDto.WineType, value, _wineDto, (_wineDto, wineType) => _wineDto.WineType = value);
        }

        /// <summary>
        /// Gets or sets Winery of wine.
        /// </summary>
        [Required(ErrorMessage = "Winery is Required")]
        public WineryDto Winery
        {
            get { return _wineDto.Winery; }
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Winery)));

                TrySetProperty(_wineDto.Winery, value, (winery) => _wineDto.Winery = winery, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
            }
        }

        /// <summary>
        /// Gets or sets wine`s list of regions.
        /// </summary>
        public List<RegionDto> Regions
        {
            get => _wineDto.Regions ??= new();
            set => SetProperty(_wineDto.Regions, value, _wineDto, (_wineDto, regions) => _wineDto.Regions = value);

        }

        /// <summary>
        /// Gets or sets wine`s list of grapes.
        /// </summary>
        public List<GrapeDto> Grapes
        {
            get => _wineDto.Grapes ??= new();
            set => SetProperty(_wineDto.Grapes, value, _wineDto, (_wineDto, grapes) => _wineDto.Grapes = value);
        }

        /// <summary>
        /// Gets IEnumerable collection of Enum WineTypes.
        /// </summary>
        public IEnumerable<WineTypes> MyEnumTypeValues => Enum.GetValues(typeof(WineTypes))
                    .Cast<WineTypes>();


        private BitmapImage _image;
        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private ICommand _createImageCommand;
        /// <summary>
        /// Create Image Command, used to convert image from user to both byte[] and BitmapImage.
        /// </summary>
        public ICommand CreateImageCommand
        {
            get
            {
                if (_createImageCommand == null)
                {
                    _createImageCommand = new RelayCommand(async () =>
                    {

                        FileOpenPicker filePicker = new()
                        {
                            ViewMode = PickerViewMode.Thumbnail,
                            SuggestedStartLocation = PickerLocationId.PicturesLibrary
                        };
                        filePicker.FileTypeFilter.Add(".jpg");
                        filePicker.FileTypeFilter.Add(".jpeg");
                        filePicker.FileTypeFilter.Add(".png");

                        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

                        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

                        Windows.Storage.StorageFile file = await filePicker.PickSingleFileAsync();
                        if (file != null)
                        {
                            using IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                            DataReader reader = new(fileStream.GetInputStreamAt(0));

                            uint LoadReader = await reader.LoadAsync((uint)fileStream.Size);

                            byte[] pixels = new byte[fileStream.Size];
                            reader.ReadBytes(pixels);

                            ByteImage = pixels;
                            OnPropertyChanged(nameof(ByteImage));

                            GetBitmapImage(pixels);
                        }
                    });
                }
                return _createImageCommand;
            }
        }

        private ICommand _addRegionCommand;
        /// <summary>
        /// Add Region Command - Adds regions to a wine.
        /// </summary>
        public ICommand AddRegionCommand
        {
            get
            {
                if (_addRegionCommand == null)
                {
                    _addRegionCommand = new RelayCommand(async () =>
                    {
                        if (SelectedRegion != null)
                        {
                            AddedRegions.Add(SelectedRegion);
                            Regions = AddedRegions.ToList();
                        }
                        else
                        {
                            RegionDto region = await _regionService.CreateRegionAsync(newRegion);
                            AddedRegions.Add(region);
                            Regions = AddedRegions.ToList();
                        }
                    });
                }
                return _addRegionCommand;
            }
        }

        private ICommand _removeRegionCommand;
        /// <summary>
        /// Remove Region Command - Removes regions to a wine.
        /// </summary>
        public ICommand RemoveRegionCommand
        {
            get
            {
                if (_removeRegionCommand == null)
                {
                    _removeRegionCommand = new RelayCommand(() =>
                    {
                        if (SelectedListRegion != null)
                        {
                            DeletedRegions.Add(SelectedListRegion);
                            AddedRegions.Remove(SelectedListRegion);
                            Regions = AddedRegions.ToList();
                        }
                    });
                }
                return _removeRegionCommand;
            }
        }

        private ICommand _addGrapeCommand;
        /// <summary>
        /// Add Grape Command - Adds grapes to a wine.
        /// </summary>
        public ICommand AddGrapeCommand
        {
            get
            {
                if (_addGrapeCommand == null)
                {
                    _addGrapeCommand = new RelayCommand(async () =>
                    {
                        if (SelectedGrape != null)
                        {
                            AddedGrapes.Add(SelectedGrape);
                            Grapes = AddedGrapes.ToList();
                        }
                        else
                        {
                            GrapeDto grape = await _grapeService.CreateGrapeAsync(newGrape);
                            AddedGrapes.Add(grape);
                            Grapes = AddedGrapes.ToList();
                        }
                    });
                }
                return _addGrapeCommand;
            }
        }

        private ICommand _removeGrapeCommand;
        /// <summary>
        /// Remove Grape Command - Removes grapes to a wine.
        /// </summary>
        public ICommand RemoveGrapeCommand
        {
            get
            {
                if (_removeGrapeCommand == null)
                {
                    _removeGrapeCommand = new RelayCommand(() =>
                    {
                        if (SelectedListGrape != null)
                        {
                            DeletedGrapes.Add(SelectedListGrape);
                            AddedGrapes.Remove(SelectedListGrape);
                            Grapes = AddedGrapes.ToList();
                        }
                    });
                }
                return _removeGrapeCommand;
            }
        }

        private ICommand _addReviewCommand;
        /// <summary>
        /// Add Review Command - Create review and add relationship to wine (many-to-many).
        /// </summary>
        public ICommand AddReviewCommand
        {
            get
            {
                if (_addReviewCommand == null)
                {
                    _addReviewCommand = new RelayCommand(async () =>
                    {
                        if (Score != 0)
                        {
                            // Need to explicit set user and datetime
                            _ratingDto.RatedBy = _userLoggedInService.User;
                            _ratingDto.DateOfRating = DateTimeOffset.Now;
                            RatingDto rating = await _ratingService.CreateRatingAsync(_ratingDto);
                            if (rating.Score == Score)
                            {
                                try
                                { 
                                WineDto wineRating = await _wineService.AddWineRating(_wineDto, rating);

                                    if (wineRating != null)
                                    {
                                        ContentDialog dialog = new()
                                        {
                                            Title = "Thank you for rating!",
                                            PrimaryButtonText = "Ok",
                                            XamlRoot = _navigationService.Frame.XamlRoot,
                                        };
                                        ContentDialogResult result = await dialog.ShowAsync();
                                        RatingsList.Add(rating);
                                        Score = 0;
                                        ReviewText = null;
                                    }
                                    else
                                    {
                                        ContentDialog dialog = new()
                                        {
                                            Title = "Something went wrong",
                                            Content = "Please try again",
                                            PrimaryButtonText = "Ok",
                                            XamlRoot = _navigationService.Frame.XamlRoot,
                                        };
                                        ContentDialogResult result = await dialog.ShowAsync();
                                    }
                                }
                                catch (HttpRequestException)
                                {
                                    ContentDialog dialog = new()
                                    {
                                        Title = "Something went wrong",
                                        Content = "Please try again",
                                        PrimaryButtonText = "Ok",
                                        XamlRoot = _navigationService.Frame.XamlRoot,
                                    };
                                    ContentDialogResult result = await dialog.ShowAsync();
                                }
                            }
                        }
                    });
                }
                return _addReviewCommand;
            }
        }

        /// <summary>
        /// Gets or sets RatingId for Rating.
        /// </summary>
        public int RatingId
        {
            get => _ratingDto.RatingId;
            set => SetProperty(_ratingDto.RatingId, value, _ratingDto, (_ratingDto, id) => _ratingDto.RatingId = value);
        }

        /// <summary>
        /// Gets or sets Score for Rating.
        /// </summary>
        public int Score
        {
            get => _ratingDto.Score;
            set
            {
                SetProperty(_ratingDto.Score, value, _ratingDto, (_ratingDto, score) => _ratingDto.Score = value);
                OnPropertyChanged(nameof(ReviewButton));
            }
        }

        /// <summary>
        /// Gets or sets Review text/description for Rating.
        /// </summary>
        public string ReviewText
        {
            get => _ratingDto.ReviewText;
            set
            {
                SetProperty(_ratingDto.ReviewText, value, _ratingDto, (_ratingDto, reviewText) => _ratingDto.ReviewText = value);
                OnPropertyChanged(nameof(ReviewButton));
            }

        }

        public bool ReviewButton => Score != 0 && ReviewText != null;

        /// <summary>
        /// Calculate average rating score for a wine.
        /// </summary>
        public double Ratings
        {
            get
            {
                int ratings = 0;
                if (_wineDto.Ratings == null)
                {
                    return 0;
                }

                foreach (RatingDto rating in _wineDto.Ratings)
                {
                    ratings += rating.Score;
                }

                double ratingPoints = 0;

                if (_wineDto.Ratings.Count > 0)
                {
                    ratingPoints = (double)ratings / _wineDto.Ratings.Count;
                }

                return Math.Round(ratingPoints, 2);

            }
        }

        /// <summary>
        /// Convert image from byte[] to BitmapImage.
        /// </summary>
        /// <param name="pixels"></param>
        private async void GetBitmapImage(byte[] byteimage)
        {
            using InMemoryRandomAccessStream stream = new();
            using (DataWriter writer = new(stream.GetOutputStreamAt(0)))
            {
                writer.WriteBytes(byteimage);
                await writer.StoreAsync();
            }
            BitmapImage image = new();
            await image.SetSourceAsync(stream);

            Image = image;
            OnPropertyChanged(nameof(Image));
        }

        /// <summary>
        /// Load all ratings/reviews for given wine and add to Ratingslist.
        /// </summary>
        private void LoadRatings()
        {
            foreach (RatingDto rating in _wineDto.Ratings)
            {
                RatingsList.Add(rating);
            }
        }
    }
}
