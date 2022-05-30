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
using WineClub.Views;

namespace WineClub.ViewModels
{
    public class WineListViewModel : ObservableRecipient, INavigationAware
    {
        private readonly IWineService _wineService;
        private readonly UserLoggedInService _userLoggedInService;
        private readonly INavigationService _navigationService;

        public WineListViewModel(INavigationService navigationService, IWineService wineService, UserLoggedInService userLoggedInService)
        {
            _wineService = wineService;
            _navigationService = navigationService;
            _userLoggedInService = userLoggedInService;
        }

        private AddWineViewModel _selected;
        public AddWineViewModel Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ObservableCollection<AddWineViewModel> Wines { get; private set; } = new ObservableCollection<AddWineViewModel>();
        public ObservableCollection<AddWineViewModel> WinesClone { get; private set; } = new ObservableCollection<AddWineViewModel>();

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(async () =>
                    {
                        AddWineViewModel addWine = new();
                        AddWinePage page = new(addWine);

                        ContentDialog dialog = new()
                        {
                            Title = "Add new wine",
                            Content = page,
                            PrimaryButtonText = "Add Wine",
                            CloseButtonText = "Cancel",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        addWine.PropertyChanged += (sender, e) => dialog.IsPrimaryButtonEnabled = !addWine.ErrorsHaveI && addWine.Year != 0;

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            WineDto wineToCreate = (WineDto)addWine;
                            List<GrapeDto> grapes = wineToCreate.Grapes;
                            List<RegionDto> regions = wineToCreate.Regions;
                            wineToCreate.AddedBy = _userLoggedInService.User;
                            wineToCreate.Grapes = null;
                            wineToCreate.Regions = null;

                            try
                            {
                                WineDto wineCreated = await _wineService.CreateWineAsync(wineToCreate);

                                foreach (GrapeDto grape in grapes)
                                {
                                    await _wineService.AddWineGrape(wineCreated, grape);
                                }

                                foreach (RegionDto region in regions)
                                {
                                    await _wineService.AddWineRegion(wineCreated, region);
                                }

                                Wines.Add(addWine);
                                Selected = addWine;
                            }
                            catch (HttpRequestException)
                            {
                                ContentDialog errorDialog = new()
                                {
                                    Title = "Error",
                                    Content = "Something went wrong, we are sorry!",
                                    PrimaryButtonText = "Ok",
                                    DefaultButton = ContentDialogButton.Primary,
                                    XamlRoot = _navigationService.Frame.XamlRoot
                                };

                                ContentDialogResult errorResult = await dialog.ShowAsync();
                            }
                        }

                    });
                }
                return _addCommand;
            }
        }

        private ICommand _editCommand;

        /// <summary>
        /// Command to edit wine, including many to many relationships.
        /// </summary>
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand<AddWineViewModel>(async param =>
                    {
                        AddWineViewModel editWine = param;
                        AddWinePage page = new(editWine);

                        ContentDialog dialog = new()
                        {
                            Title = "Edit wine",
                            Content = page,
                            PrimaryButtonText = "Edit Wine",
                            CloseButtonText = "Cancel",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        editWine.PropertyChanged += (sender, e) => dialog.IsPrimaryButtonEnabled = !editWine.ErrorsHaveI;

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            WineDto wineToEdit = (WineDto)editWine;
                            ObservableCollection<GrapeDto> grapes = editWine.AddedGrapes;
                            ObservableCollection<RegionDto> regions = editWine.AddedRegions;
                            wineToEdit.Grapes = null;
                            wineToEdit.Regions = null;
                            List<GrapeDto> deletedGrapes = editWine.DeletedGrapes;
                            List<RegionDto> deletesRegions = editWine.DeletedRegions;

                            // Delete grapes
                            foreach (GrapeDto grape in deletedGrapes)
                            {
                                if (!grapes.Contains(grape))
                                {
                                    _ = await _wineService.DeleteWineGrape(wineToEdit, grape);
                                }
                            }

                            // Delete regions
                            foreach (RegionDto region in deletesRegions)
                            {
                                if (!regions.Contains(region))
                                {
                                    _ = await _wineService.DeleteWineRegion(wineToEdit, region);
                                }
                            }

                            // Add new grapes
                            foreach (GrapeDto grape in grapes)
                            {
                                try
                                {
                                    _ = await _wineService.AddWineGrape(wineToEdit, grape);
                                }
                                catch (HttpRequestException)
                                {
                                    ContentDialog error = new()
                                    {
                                        Title = "Error!",
                                        Content = "Something went wrong, please try again.",
                                        CloseButtonText = "Ok",
                                        DefaultButton = ContentDialogButton.Primary,
                                        XamlRoot = _navigationService.Frame.XamlRoot
                                    };

                                    ContentDialogResult errorResult = await dialog.ShowAsync();
                                }
                            }

                            // Add new regions
                            foreach (RegionDto region in regions)
                            {
                                try
                                {
                                    _ = await _wineService.AddWineRegion(wineToEdit, region);
                                }
                                catch (HttpRequestException)
                                {
                                    ContentDialog error = new()
                                    {
                                        Title = "Error!",
                                        Content = "Something went wrong, please try again.",
                                        CloseButtonText = "Ok",
                                        DefaultButton = ContentDialogButton.Primary,
                                        XamlRoot = _navigationService.Frame.XamlRoot
                                    };

                                    ContentDialogResult errorResult = await dialog.ShowAsync();
                                }
                            }

                            if (await _wineService.EditWineAsync(wineToEdit))
                            {
                                int index = Wines.IndexOf(param);
                                IEnumerable<WineDto> wines = await _wineService.GetWinesAsync();
                                WineDto wineUpdated = wines.FirstOrDefault(x => x.WineId == editWine.WineId);
                                Wines.RemoveAt(index);
                                Wines.Insert(index, new AddWineViewModel(wineUpdated));
                                Selected = Wines.FirstOrDefault(x => x.WineId == wineUpdated.WineId);
                            }
                        }

                    }, param => param != null);
                }
                return _editCommand;
            }
        }

        private ICommand _deleteCommand;

        /// <summary>
        /// Command to delete selected wine.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<AddWineViewModel>(async param =>
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "Delete wine",
                            Content = "If you delete this wine, you won't be able to recover it. Do you want to delete it?",
                            PrimaryButtonText = "Delete wine",
                            CloseButtonText = "Cancel",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = _navigationService.Frame.XamlRoot
                        };

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            if (await _wineService.DeleteWineAsync((WineDto)param))
                            {
                                Wines.Remove(param);
                            }
                        }

                    }, param => param != null);
                }
                return _deleteCommand;
            }
        }

        /// <summary>
        /// Event handler to check for text changed when searching for wines in list view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AutoSuggestBoxWine_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Wines.Clear();
                string[] splitText = sender.Text.ToLower().Split(" ");
                foreach (AddWineViewModel item in WinesClone)
                {
                    bool found = splitText.All((key) =>
                    {
                        return item.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        Wines.Add(item);
                    }
                }
            }
        }

        /// <summary>
        ///  Login required when wite gets navighated to. Also populates list view.
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

            if (Wines.Count == 0)
            {
                IEnumerable<WineDto> wines = await _wineService.GetWinesAsync();

                foreach (WineDto wine in wines)
                {
                    Wines.Add(new AddWineViewModel(wine));
                    WinesClone.Add(new AddWineViewModel(wine));
                }
            }
            EnsureItemSelected();
        }

        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// To make sure a wine is selected if you visit WineList
        /// </summary>
        public void EnsureItemSelected()
        {
            if (Selected == null && Wines.Count > 0)
            {
                if (_userLoggedInService.WineButtonMainPage != null)
                {
                    Selected = Wines.FirstOrDefault(x => x.WineId == _userLoggedInService.WineButtonMainPage.WineId);
                    _userLoggedInService.WineButtonMainPage = null;
                }
                else
                {
                    Selected = Wines.First();
                }

            }
        }
    }
}
