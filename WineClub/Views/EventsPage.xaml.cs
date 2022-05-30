using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class EventsPage : Page
    {
        public EventsViewModel ViewModel { get; }

        public EventsPage()
        {
            ViewModel = Ioc.Default.GetService<EventsViewModel>();
            InitializeComponent();
        }
    }
}
