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
    /// 应用程序入口点，继承自 PrismApplication 以提供 MVVM 和 DI 支持
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 全局配置属性
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        public App()
        {
            // 初始化配置加载器，读取 appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // 配置 Serilog 日志记录器
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("应用程序正在启动...");

            // 全局未捕获异常处理
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Log.Fatal(e.ExceptionObject as Exception, "AppDomain 未捕获的异常");

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.Fatal(e.Exception, "未观测到的 Task 异常");
                e.SetObserved();
            };
        }

        /// <summary>
        /// 启动时触发的操作
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // UI 线程异常捕获
            this.DispatcherUnhandledException += (s, ex) =>
            {
                Log.Fatal(ex.Exception, "UI Dispatcher 未捕获的异常");
                MessageBox.Show("发生严重错误，请查看日志文件。", "严重错误", MessageBoxButton.OK, MessageBoxImage.Error);
                ex.Handled = true; 
            };
        }

        /// <summary>
        /// 程序退出时释放资源
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("应用程序正在退出...");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        /// <summary>
        /// 创建应用程序主外壳窗口
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 初始化完成后执行，进行数据库检查及显示登录界面
        /// </summary>
        protected override void OnInitialized()
        {
            var userService = Container.Resolve<IUserService>();
            userService.EnsureDatabaseCreated();

            var authService = Container.Resolve<IAuthService>();
            authService.SeedDefaultAdmin();

            ShowLoginView();
        }

        /// <summary>
        /// 显示登录窗口的逻辑
        /// </summary>
        private void ShowLoginView()
        {
            var loginView = Container.Resolve<LoginView>();
            var loginViewModel = (LoginViewModel)loginView.DataContext;
            loginViewModel.Reset();

            // 订阅登录成功事件
            loginViewModel.LoginSuccess += () =>
            {
                var shell = Application.Current.MainWindow;
                if (shell == null) return;

                var shellViewModel = (MainWindowViewModel)shell.DataContext;
                shellViewModel.Reset();

                // 先取消订阅以防止重复绑定
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

        /// <summary>
        /// 注册服务及视图
        /// </summary>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册配置实例
            containerRegistry.RegisterInstance<IConfiguration>(Configuration);

            // 注册业务服务为单例
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
            containerRegistry.RegisterSingleton<IBusyService, BusyService>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IThemeService, ThemeService>();
            containerRegistry.RegisterSingleton<ICodeGenService, CodeGenService>();

            // 注册导航视图
            containerRegistry.RegisterForNavigation<DashboardView>();
            containerRegistry.RegisterForNavigation<UserListView>();
            containerRegistry.RegisterForNavigation<UserEditView>();
            containerRegistry.RegisterForNavigation<CodeGenView>();
        }
    }
}
