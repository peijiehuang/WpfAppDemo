using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Serilog;
using Microsoft.Extensions.Configuration;
using WpfAppDemo.Views;
using WpfAppDemo.Services;
using WpfAppDemo.ViewModels;

namespace WpfAppDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public IConfiguration Configuration { get; private set; }

        public App()
        {
            // Initialize Configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting...");

            // Global Exception Handling
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Log.Fatal(e.ExceptionObject as Exception, "AppDomain Unhandled Exception");

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.Fatal(e.Exception, "Unobserved Task Exception");
                e.SetObserved();
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += (s, ex) =>
            {
                Log.Fatal(ex.Exception, "UI Dispatcher Unhandled Exception");
                MessageBox.Show("A critical error occurred. Please check the logs.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ex.Handled = true; // Prevents crash, but might leave app in unstable state depending on error
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Application Exiting...");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var userService = Container.Resolve<IUserService>();
            userService.EnsureDatabaseCreated();

            var authService = Container.Resolve<IAuthService>();
            authService.SeedDefaultAdmin();

            ShowLoginView();
        }

        private void ShowLoginView()
        {
            var loginView = Container.Resolve<LoginView>();
            var loginViewModel = (LoginViewModel)loginView.DataContext;
            loginViewModel.Reset();

            loginViewModel.LoginSuccess += () =>
            {
                var shell = Application.Current.MainWindow;
                if (shell == null) return;

                var shellViewModel = (MainWindowViewModel)shell.DataContext;
                shellViewModel.Reset();

                // Unsubscribe first to avoid multiple handlers
                shellViewModel.LogoutRequested -= OnLogoutRequested;
                shellViewModel.LogoutRequested += OnLogoutRequested;

                void OnLogoutRequested()
                {
                    shell.Hide();
                    ShowLoginView();
                }

                shell.Show();
                loginView.Close();
            };

            loginView.Show();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Configuration
            containerRegistry.RegisterInstance<IConfiguration>(Configuration);

            // Services
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
            containerRegistry.RegisterSingleton<IBusyService, BusyService>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IThemeService, ThemeService>();

            // Navigation Views
            containerRegistry.RegisterForNavigation<DashboardView>();
            containerRegistry.RegisterForNavigation<UserListView>();
            containerRegistry.RegisterForNavigation<UserEditView>();
        }
    }
}
