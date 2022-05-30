
using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class AddWinePage : Page
    {
        public AddWineViewModel ViewModel { get; }

        public AddWinePage(AddWineViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
