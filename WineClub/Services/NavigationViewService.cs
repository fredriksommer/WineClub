using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WineClub.Contracts.Services;
using WineClub.Helpers;
using WineClub.ViewModels;

namespace WineClub.Services
{
    public class NavigationViewService : INavigationViewService
    {
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private NavigationView _navigationView;

        public IList<object> MenuItems
            => _navigationView.MenuItems;

        public object SettingsItem
            => _navigationView.SettingsItem;

        public NavigationViewService(INavigationService navigationService, IPageService pageService)
        {
            _navigationService = navigationService;
            _pageService = pageService;
        }

        public void Initialize(NavigationView navigationView)
        {
            _navigationView = navigationView;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
        }

        public void UnregisterEvents()
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }

        public NavigationViewItem GetSelectedItem(Type pageType)
            => GetSelectedItem(_navigationView.MenuItems, pageType);

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
            => _navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.NavigateTo(typeof(SettingsViewModel).FullName);
            }
            else
            {
                NavigationViewItem selectedItem = args.InvokedItemContainer as NavigationViewItem;
                string pageKey = selectedItem.GetValue(NavHelper.NavigateToProperty) as string;

                if (pageKey != null)
                {
                    _navigationService.NavigateTo(pageKey);
                }
            }
        }

        private NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (NavigationViewItem item in menuItems.OfType<NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                NavigationViewItem selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            string pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            if (pageKey != null)
            {
                return _pageService.GetPageType(pageKey) == sourcePageType;
            }

            return false;
        }
    }
}
