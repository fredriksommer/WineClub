using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class SignUpPage : Page
    {
        public SignUpViewModel ViewModel { get; }

        public SignUpPage()
        {
            ViewModel = Ioc.Default.GetService<SignUpViewModel>();
            InitializeComponent();
        }
    }
}
