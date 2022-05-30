using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WineClub.Activation;
using WineClub.Contracts.Services;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Services;
using WineClub.Helpers;
using WineClub.Services;
using WineClub.ViewModels;
using WineClub.Views;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace WineClub
{
    public partial class App : Application
    {
        public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices());
        }

        private async void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            ContentDialog dialog = new()
            {
                Title = "Ops!",
                Content = "Something went wrong",
                PrimaryButtonText = "Ok",
                XamlRoot = Ioc.Default.GetService<INavigationService>().Frame.XamlRoot,
            };

            _ = await dialog.ShowAsync();

            Ioc.Default.GetService<INavigationService>().NavigateTo(typeof(MainViewModel).FullName);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            IActivationService activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
        }

        private System.IServiceProvider ConfigureServices()
        {

            ServiceCollection services = new();

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IWineService, WineService>();
            services.AddSingleton<IWineryService, WineryService>();
            services.AddSingleton<IRegionService, RegionService>();
            services.AddSingleton<IGrapeService, GrapeService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IWineEventService, WineEventService>();
            services.AddSingleton<IRatingService, RatingService>();

            // Login Service
            services.AddSingleton<UserLoggedInService>();

            // Views and ViewModels
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<WineListViewModel>();
            services.AddTransient<WineListPage>();
            services.AddTransient<AddWineViewModel>();
            services.AddTransient<AddWinePage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<SignUpPage>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ProfilePage>();
            services.AddTransient<EventsViewModel>();
            services.AddTransient<EventsPage>();
            services.AddTransient<AddEditEventViewModel>();
            services.AddTransient<AddEditEventPage>();
            services.AddTransient<EventDetailedViewModel>();
            services.AddTransient<EventDetailedPage>();
            return services.BuildServiceProvider();
        }
    }
}
