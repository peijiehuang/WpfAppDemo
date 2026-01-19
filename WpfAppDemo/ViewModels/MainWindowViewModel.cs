using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WpfAppDemo.Services; // Assuming ILocalizationService is in this namespace

namespace WpfAppDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public IBusyService BusyService { get; }
        private string _title = "WPF Prism Scaffolding";
        private MenuItem? _selectedMenuItem;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MenuItem? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (SetProperty(ref _selectedMenuItem, value) && value != null)
                {
                    Navigate(value.NavigationPath);
                }
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; }
        public DelegateCommand<string> SwitchLanguageCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand ToggleThemeCommand { get; }

        public event Action? LogoutRequested;

        public MainWindowViewModel(IRegionManager regionManager, ILocalizationService localizationService, IBusyService busyService, IThemeService themeService, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _regionManager = regionManager;
            _localizationService = localizationService;
            _themeService = themeService;
            _configuration = configuration;
            BusyService = busyService;

            _localizationService.LanguageChanged += OnLanguageChanged;

            MenuItems = new ObservableCollection<MenuItem>();
            InitializeMenuItems();

            SwitchLanguageCommand = new DelegateCommand<string>(OnSwitchLanguage);
            LogoutCommand = new DelegateCommand(OnLogout);
            ToggleThemeCommand = new DelegateCommand(OnToggleTheme);

            // Use Dispatcher to ensure the view is loaded and regions are registered before initial navigation
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                SelectedMenuItem = MenuItems.FirstOrDefault();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        public void Reset()
        {
            SelectedMenuItem = MenuItems.FirstOrDefault();
        }

        private void InitializeMenuItems()
        {
            MenuItems.Clear();
            MenuItems.Add(new MenuItem("Shell_Dashboard", "ViewDashboard", "DashboardView"));
            MenuItems.Add(new MenuItem("Shell_Users", "AccountGroup", "UserListView"));
            MenuItems.Add(new MenuItem("Menu_TEST", "Database", "TESTListView"));
            if (_configuration.GetValue<bool>("AppSettings:EnableCodeGen"))
            {
                MenuItems.Add(new MenuItem("Shell_CodeGen", "CodeArray", "CodeGenView"));
            }
        }

        private void OnLanguageChanged()
        {
            InitializeMenuItems();
            // Also notify that Title properties might have changed if any
            RaisePropertyChanged(nameof(MenuItems));
        }

        private void OnLogout()
        {
            Serilog.Log.Information("User requested logout.");
            LogoutRequested?.Invoke();
        }

        private void OnToggleTheme()
        {
            _themeService.ToggleTheme();
        }

        private void OnSwitchLanguage(string languageCode)
        {
            Serilog.Log.Information("Language switched to {Language}", languageCode);
            _localizationService.SetLanguage(languageCode);
        }

        private void Navigate(string path)
        {
            Serilog.Log.Debug("Navigating to {Path}", path);
            _regionManager.RequestNavigate("ContentRegion", path);
        }
    }

    public class MenuItem
    {
        public string Title { get; }
        public string Icon { get; }
        public string NavigationPath { get; }

        public MenuItem(string title, string icon, string navigationPath)
        {
            Title = title;
            Icon = icon;
            NavigationPath = navigationPath;
        }
    }
}
