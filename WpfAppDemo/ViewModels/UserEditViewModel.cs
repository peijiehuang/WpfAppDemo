using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class UserEditViewModel : BindableBase, INavigationAware
    {
        private readonly IUserService _userService;
        private readonly IRegionManager _regionManager;
        private User _user = new();
        private bool _isEditMode;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string Title => _isEditMode 
            ? (System.Windows.Application.Current.TryFindResource("User_EditTitle")?.ToString() ?? "Edit User")
            : (System.Windows.Application.Current.TryFindResource("User_AddTitle")?.ToString() ?? "Add User");

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public UserEditViewModel(IUserService userService, IRegionManager regionManager, ILocalizationService localizationService)
        {
            _userService = userService;
            _regionManager = regionManager;
            localizationService.LanguageChanged += () => RaisePropertyChanged(nameof(Title));

            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnSave()
        {
            if (_isEditMode)
                _userService.UpdateUser(User);
            else
                _userService.AddUser(User);

            NavigateBack();
        }

        private void OnCancel()
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            _regionManager.RequestNavigate("ContentRegion", "UserListView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("User"))
            {
                var user = navigationContext.Parameters.GetValue<User>("User");
                User = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };
                _isEditMode = true;
            }
            else
            {
                User = new User();
                _isEditMode = false;
            }
            RaisePropertyChanged(nameof(Title));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
