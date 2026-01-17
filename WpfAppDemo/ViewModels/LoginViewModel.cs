using Prism.Commands;
using Prism.Mvvm;
using System;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly ILocalizationService _localizationService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand<string> SwitchLanguageCommand { get; }

        public event Action? LoginSuccess;

        public LoginViewModel(IAuthService authService, ILocalizationService localizationService)
        {
            _authService = authService;
            _localizationService = localizationService;
            localizationService.LanguageChanged += () =>
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    ErrorMessage = System.Windows.Application.Current.TryFindResource("Login_Error")?.ToString() ?? "Login Failed";
            };
            LoginCommand = new DelegateCommand(OnLogin);
            SwitchLanguageCommand = new DelegateCommand<string>(OnSwitchLanguage);
        }

        private void OnSwitchLanguage(string languageCode)
        {
            _localizationService.SetLanguage(languageCode);
        }

        public void Reset()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        private void OnLogin()
        {
            if (_authService.Login(Username, Password))
            {
                ErrorMessage = string.Empty;
                LoginSuccess?.Invoke();
            }
            else
            {
                ErrorMessage = System.Windows.Application.Current.TryFindResource("Login_Error")?.ToString() ?? "Login Failed";
            }
        }
    }
}
