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
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount;

        public ObservableCollection<User> Users { get; } = new();

        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
        public int PageIndex { get => _pageIndex; set => SetProperty(ref _pageIndex, value); }
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    PageIndex = 1;
                    LoadUsersAsync();
                }
            }
        }
        public int TotalCount { get => _totalCount; set => SetProperty(ref _totalCount, value); }

        public DelegateCommand AddUserCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand ExportCommand { get; }
        public DelegateCommand PrevPageCommand { get; }
        public DelegateCommand NextPageCommand { get; }
        public DelegateCommand<User> EditUserCommand { get; }
        public DelegateCommand<User> DeleteUserCommand { get; }

        public UserListViewModel(IUserService userService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _userService = userService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;

            AddUserCommand = new DelegateCommand(OnAddUser);
            SearchCommand = new DelegateCommand(() => { PageIndex = 1; LoadUsersAsync(); });
            ExportCommand = new DelegateCommand(OnExport);
            PrevPageCommand = new DelegateCommand(() => { if (PageIndex > 1) { PageIndex--; LoadUsersAsync(); } });
            NextPageCommand = new DelegateCommand(() => { if (PageIndex * PageSize < TotalCount) { PageIndex++; LoadUsersAsync(); } });
            EditUserCommand = new DelegateCommand<User>(OnEditUser);
            DeleteUserCommand = new DelegateCommand<User>(OnDeleteUser);
        }

        private async void LoadUsersAsync()
        {
            try
            {
                _busyService.Busy("正在查询用户...");
                await Task.CompletedTask;
                
                int total = 0;
                var data = _userService.GetUsers(PageIndex, PageSize, SearchText, ref total);
                TotalCount = total;

                Users.Clear();
                foreach (var user in data)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "加载用户列表失败");
                await _messageService.ShowMessageAsync($"查询失败: {ex.Message}", "错误");
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
                var sfd = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"用户导出_{DateTime.Now:yyyyMMddHHmm}" };
                if (sfd.ShowDialog() == true)
                {
                    _busyService.Busy("正在导出...");
                    await Task.Run(() => {
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

        private void OnAddUser() => _regionManager.RequestNavigate("ContentRegion", "UserEditView");
        private void OnEditUser(User user) => _regionManager.RequestNavigate("ContentRegion", "UserEditView", new NavigationParameters { { "User", user } });

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
