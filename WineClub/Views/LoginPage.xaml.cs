using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginViewModel ViewModel { get; }

        public LoginPage()
        {
            ViewModel = Ioc.Default.GetService<LoginViewModel>();
            InitializeComponent();
        }
    }
}
