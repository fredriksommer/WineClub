using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WineClub.Contracts.Services;
using WineClub.ViewModels;
using WineClub.Views;

namespace WineClub.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        public PageService()
        {
            Configure<MainViewModel, MainPage>();
            Configure<SettingsViewModel, SettingsPage>();
            Configure<WineListViewModel, WineListPage>();
            Configure<AddWineViewModel, AddWinePage>();
            Configure<LoginViewModel, LoginPage>();
            Configure<SignUpViewModel, SignUpPage>();
            Configure<ProfileViewModel, ProfilePage>();
            Configure<EventsViewModel, EventsPage>();
            Configure<AddEditEventViewModel, AddEditEventPage>();
            Configure<EventDetailedViewModel, EventDetailedPage>();
        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }

            return pageType;
        }

        private void Configure<VM, V>()
            where VM : ObservableObject
            where V : Page
        {
            lock (_pages)
            {
                string key = typeof(VM).FullName;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                Type type = typeof(V);
                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}
