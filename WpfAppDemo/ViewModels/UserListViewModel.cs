using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
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
        private string _searchText = string.Empty;

        public ObservableCollection<User> Users { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public DelegateCommand AddUserCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand ExportCommand { get; }
        public DelegateCommand<User> EditUserCommand { get; }
        public DelegateCommand<User> DeleteUserCommand { get; }

        public UserListViewModel(IUserService userService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _userService = userService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;

            AddUserCommand = new DelegateCommand(OnAddUser);
            SearchCommand = new DelegateCommand(LoadUsersAsync);
            ExportCommand = new DelegateCommand(OnExport);
            EditUserCommand = new DelegateCommand<User>(OnEditUser);
            DeleteUserCommand = new DelegateCommand<User>(OnDeleteUser);
        }

        private async void LoadUsersAsync()
        {
            try
            {
                _busyService.Busy("正在查询用户...");
                Users.Clear();
                foreach (var user in _userService.GetUsers(SearchText))
                {
                    Users.Add(user);
                }
            }
            finally
            {
                _busyService.Idle();
            }
        }

        private async void OnExport()
        {
            try
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"用户列表导出_{DateTime.Now:yyyyMMddHHmm}"
                };

                if (sfd.ShowDialog() == true)
                {
                    _busyService.Busy("正在导出...");
                    await Task.Run(() =>
                    {
                        using var stream = _userService.ExportUsers(SearchText);
                        using var fileStream = File.Create(sfd.FileName);
                        stream.CopyTo(fileStream);
                    });
                    _busyService.Idle();
                    await _messageService.ShowMessageAsync("数据已成功导出", "Common_Info");
                }
            }
            catch (Exception ex)
            {
                _busyService.Idle();
                await _messageService.ShowMessageAsync($"导出失败: {ex.Message}", "Common_Error");
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
            if (await _messageService.ShowConfirmationAsync($"确定要删除用户 '{user.Username}' 吗?", "Common_DeleteConfirm"))
            {
                _userService.DeleteUser(user.Id);
                LoadUsersAsync();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext) => LoadUsersAsync();
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}