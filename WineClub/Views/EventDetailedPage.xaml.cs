using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class EventDetailedPage : Page
    {
        public EventDetailedViewModel ViewModel { get; }

        public EventDetailedPage()
        {
            ViewModel = Ioc.Default.GetService<EventDetailedViewModel>();
            InitializeComponent();
        }
    }
}
