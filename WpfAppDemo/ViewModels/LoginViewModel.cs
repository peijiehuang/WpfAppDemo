using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfAppDemo.Common;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    /// <summary>
    /// 登录界面视图模型
    /// </summary>
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly ILocalizationService _localizationService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        /// <summary>
        /// 登录成功事件
        /// </summary>
        public event Action? LoginSuccess;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authService">认证服务</param>
        /// <param name="localizationService">本地化服务</param>
        public LoginViewModel(IAuthService authService, ILocalizationService localizationService)
        {
            _authService = authService;
            _localizationService = localizationService;
            _localizationService.LanguageChanged += OnLanguageChanged;
            
            Title = "系统登录";
        }

        private void OnLanguageChanged()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                UpdateErrorMessage();
        }

        private void UpdateErrorMessage()
        {
            ErrorMessage = Application.Current.TryFindResource("Login_Error")?.ToString() ?? "登录失败";
        }

        /// <summary>
        /// 切换语言命令
        /// </summary>
        /// <param name="languageCode">语言代码</param>
        [RelayCommand]
        private void SwitchLanguage(string languageCode)
        {
            _localizationService.SetLanguage(languageCode);
        }

        /// <summary>
        /// 重置输入
        /// </summary>
        public void Reset()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// 登录命令执行逻辑
        /// </summary>
        [RelayCommand]
        private void Login()
        {
            if (_authService.Login(Username, Password))
            {
                ErrorMessage = string.Empty;
                LoginSuccess?.Invoke();
            }
            else
            {
                UpdateErrorMessage();
            }
        }
    }
}