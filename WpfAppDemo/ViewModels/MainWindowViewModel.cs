using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Prism.Regions;
using Serilog;
using WpfAppDemo.Common;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    /// <summary>
    /// 主窗口视图模型，负责全局导航和主菜单管理
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 忙碌状态服务，通过属性暴露给 UI 绑定
        /// </summary>
        public IBusyService BusyService { get; }

        [ObservableProperty]
        private MenuItem? _selectedMenuItem;

        /// <summary>
        /// 菜单项集合
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems { get; } = new();

        /// <summary>
        /// 注销请求事件
        /// </summary>
        public event Action? LogoutRequested;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel(
            IRegionManager regionManager, 
            ILocalizationService localizationService, 
            IBusyService busyService, 
            IThemeService themeService, 
            IConfiguration configuration)
        {
            _regionManager = regionManager;
            _localizationService = localizationService;
            _themeService = themeService;
            _configuration = configuration;
            BusyService = busyService;

            Title = "WPF 优雅开发示例";

            _localizationService.LanguageChanged += OnLanguageChanged;

            InitializeMenuItems();

            // 延迟加载初始页面，确保 Region 注册完成
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SelectedMenuItem = MenuItems.FirstOrDefault();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        /// <summary>
        /// 当选中项变更时触发导航
        /// </summary>
        partial void OnSelectedMenuItemChanged(MenuItem? value)
        {
            if (value != null)
            {
                Navigate(value.NavigationPath);
            }
        }

        /// <summary>
        /// 重置主界面状态
        /// </summary>
        public void Reset()
        {
            SelectedMenuItem = MenuItems.FirstOrDefault();
        }

        private void InitializeMenuItems()
        {
            MenuItems.Clear();
            MenuItems.Add(new MenuItem("Shell_Dashboard", "ViewDashboard", "DashboardView"));
            MenuItems.Add(new MenuItem("Shell_Users", "AccountGroup", "UserListView"));
            MenuItems.Add(new MenuItem("Menu_Test", "Database", "TestListView"));
            if (_configuration.GetValue<bool>("AppSettings:EnableCodeGen"))
            {
                MenuItems.Add(new MenuItem("Shell_CodeGen", "CodeArray", "CodeGenView"));
            }
        }

        private void OnLanguageChanged()
        {
            InitializeMenuItems();
            OnPropertyChanged(nameof(MenuItems));
        }

        /// <summary>
        /// 注销登录命令
        /// </summary>
        [RelayCommand]
        private void Logout()
        {
            Log.Information("用户请求注销。");
            LogoutRequested?.Invoke();
        }

        /// <summary>
        /// 切换主题命令
        /// </summary>
        [RelayCommand]
        private void ToggleTheme()
        {
            _themeService.ToggleTheme();
        }

        /// <summary>
        /// 切换语言命令
        /// </summary>
        [RelayCommand]
        private void SwitchLanguage(string languageCode)
        {
            Log.Information("切换语言至: {Language}", languageCode);
            _localizationService.SetLanguage(languageCode);
        }

        private void Navigate(string path)
        {
            Log.Debug("导航至: {Path}", path);
            _regionManager.RequestNavigate("ContentRegion", path);
        }
    }

    /// <summary>
    /// 菜单项模型
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 标题（资源键）
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 图标名称 (MaterialDesign)
        /// </summary>
        public string Icon { get; }
        /// <summary>
        /// 导航路径
        /// </summary>
        public string NavigationPath { get; }

        public MenuItem(string title, string icon, string navigationPath)
        {
            Title = title;
            Icon = icon;
            NavigationPath = navigationPath;
        }
    }
}