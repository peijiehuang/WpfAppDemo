using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using Serilog;
using WpfAppDemo.Common;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    /// <summary>
    /// 用户列表界面视图模型
    /// </summary>
    public partial class UserListViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IRegionManager _regionManager;
        private readonly IBusyService _busyService;
        private readonly IMessageService _messageService;
        
        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private int _pageIndex = 1;

        [ObservableProperty]
        private int _pageSize = 20;

        [ObservableProperty]
        private int _totalCount;

        /// <summary>
        /// 用户数据集合
        /// </summary>
        public ObservableCollection<User> Users { get; } = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserListViewModel(IUserService userService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _userService = userService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;
            Title = "用户管理";
        }

        /// <summary>
        /// 当页码改变时重新加载数据
        /// </summary>
        partial void OnPageIndexChanged(int value) => LoadUsersAsync();

        /// <summary>
        /// 当每页显示数量改变时重置页码并重新加载
        /// </summary>
        partial void OnPageSizeChanged(int value)
        {
            PageIndex = 1;
            LoadUsersAsync();
        }

        /// <summary>
        /// 异步加载用户列表
        /// </summary>
        [RelayCommand]
        private async Task LoadUsersAsync()
        {
            try
            {
                _busyService.Busy("正在查询用户...");
                // 模拟异步操作
                await Task.Delay(100); 
                
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
                Log.Error(ex, "加载用户列表失败");
                await _messageService.ShowMessageAsync($"查询失败: {ex.Message}", "Common_Error");
            }
            finally
            {
                _busyService.Idle();
            }
        }

        /// <summary>
        /// 搜索命令
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            PageIndex = 1;
            LoadUsersAsync();
        }

        /// <summary>
        /// 导出 Excel 命令
        /// </summary>
        [RelayCommand]
        private async Task Export()
        {
            try
            {
                var sfd = new SaveFileDialog { Filter = "Excel 文件 (*.xlsx)|*.xlsx", FileName = $"用户导出_{DateTime.Now:yyyyMMddHHmm}" };
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

        /// <summary>
        /// 上一页命令
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanPrevPage))]
        private void PrevPage() => PageIndex--;
        private bool CanPrevPage() => PageIndex > 1;

        /// <summary>
        /// 下一页命令
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanNextPage))]
        private void NextPage() => PageIndex++;
        private bool CanNextPage() => PageIndex * PageSize < TotalCount;

        /// <summary>
        /// 跳转至添加用户界面
        /// </summary>
        [RelayCommand]
        private void AddUser() => _regionManager.RequestNavigate("ContentRegion", "UserEditView");

        /// <summary>
        /// 跳转至编辑用户界面
        /// </summary>
        [RelayCommand]
        private void EditUser(User user) => _regionManager.RequestNavigate("ContentRegion", "UserEditView", new NavigationParameters { { "User", user } });

        /// <summary>
        /// 删除用户命令
        /// </summary>
        [RelayCommand]
        private async Task DeleteUser(User user)
        {
            if (await _messageService.ShowConfirmationAsync($"确定要删除用户 '{user.Username}' 吗?", "Common_DeleteConfirm"))
            {
                _userService.DeleteUser(user.Id);
                await LoadUsersAsync();
            }
        }

        /// <summary>
        /// 导航进入页面时刷新列表
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            LoadUsersAsync();
        }
    }
}