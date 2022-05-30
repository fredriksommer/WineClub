using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class WineListPage : Page
    {
        public WineListViewModel ViewModel { get; }

        public WineListPage()
        {
            ViewModel = Ioc.Default.GetService<WineListViewModel>();
            InitializeComponent();
        }

        private void OnViewStateChanged(object sender, ListDetailsViewState e)
        {
            if (e == ListDetailsViewState.Both)
            {
                ViewModel.EnsureItemSelected();
            }
        }
    }
}
