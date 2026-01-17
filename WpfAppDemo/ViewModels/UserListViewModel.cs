using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class UserListViewModel : BindableBase, INavigationAware
    {
        private readonly IUserService _userService;
        private readonly IRegionManager _regionManager;
        private readonly IBusyService _busyService;
        private readonly IMessageService _messageService;

        public ObservableCollection<User> Users { get; } = new();

        public DelegateCommand AddUserCommand { get; }
        public DelegateCommand<User> EditUserCommand { get; }
        public DelegateCommand<User> DeleteUserCommand { get; }

        public UserListViewModel(IUserService userService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _userService = userService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;

            AddUserCommand = new DelegateCommand(OnAddUser);
            EditUserCommand = new DelegateCommand<User>(OnEditUser);
            DeleteUserCommand = new DelegateCommand<User>(OnDeleteUser);

            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            try
            {
                _busyService.Busy("Loading users...");
                await Task.Delay(800); // Simulate network/DB delay
                Users.Clear();
                foreach (var user in _userService.GetUsers())
                {
                    Users.Add(user);
                }
            }
            finally
            {
                _busyService.Idle();
            }
        }

        private void OnAddUser()
        {
            _regionManager.RequestNavigate("ContentRegion", "UserEditView");
        }

        private void OnEditUser(User user)
        {
            var parameters = new NavigationParameters { { "User", user } };
            _regionManager.RequestNavigate("ContentRegion", "UserEditView", parameters);
        }

        private async void OnDeleteUser(User user)
        {
            if (await _messageService.ShowConfirmationAsync($"Are you sure you want to delete user '{user.Username}'?", "Delete Confirm"))
            {
                _userService.DeleteUser(user.Id);
                LoadUsersAsync();
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadUsersAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
