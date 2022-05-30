
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Services;
using WineClub.Views;

namespace WineClub.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private string _loginVisibility;

        public INavigationService NavigationService { get; }

        public INavigationViewService NavigationViewService { get; }

        public UserLoggedInService _loggedInService;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public string LoginVisibilty
        {
            get => _loginVisibility ??= "Visible";
            set => SetProperty(ref _loginVisibility, value);
        }


        public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, UserLoggedInService userLoggedInService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            NavigationViewService = navigationViewService;
            _loggedInService = Ioc.Default.GetService<UserLoggedInService>();
        }

        private ICommand _logOutCommand;
        public ICommand LogOutCommand
        {
            get
            {
                if (_logOutCommand == null)
                {
                    _logOutCommand = new RelayCommand(async () =>
                    {
                        ContentDialog dialog = new()
                        {
                            Title = "Log Out",
                            Content = "Are you sure you want to log out?",
                            PrimaryButtonText = "Log out",
                            CloseButtonText = "Cancel",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = NavigationService.Frame.XamlRoot,
                        };

                        ContentDialogResult result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            _loggedInService.LogOut();
                            NavigationService.NavigateTo(typeof(MainViewModel).FullName);
                        }
                    });
                }
                return _logOutCommand;
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = NavigationViewService.SettingsItem;
                return;
            }

            NavigationViewItem selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }
    }
}
