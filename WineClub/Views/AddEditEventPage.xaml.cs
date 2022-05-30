
using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class AddEditEventPage : Page
    {
        public AddEditEventViewModel ViewModel { get; }

        public AddEditEventPage(AddEditEventViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
