using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using WpfAppDemo.Common;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    /// <summary>
    /// 用户编辑/新增界面视图模型
    /// </summary>
    public partial class UserEditViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IRegionManager _regionManager;
        private bool _isEditMode;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayText))]
        private User _user = new();

        /// <summary>
        /// 界面显示的动态标题
        /// </summary>
        public string DisplayText => _isEditMode 
            ? (Application.Current.TryFindResource("User_EditTitle")?.ToString() ?? "编辑用户")
            : (Application.Current.TryFindResource("User_AddTitle")?.ToString() ?? "新增用户");

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserEditViewModel(IUserService userService, IRegionManager regionManager, ILocalizationService localizationService)
        {
            _userService = userService;
            _regionManager = regionManager;
            
            // 订阅语言变更，动态刷新标题
            localizationService.LanguageChanged += () => OnPropertyChanged(nameof(DisplayText));
        }

        /// <summary>
        /// 保存用户命令
        /// </summary>
        [RelayCommand]
        private void Save()
        {
            if (_isEditMode)
                _userService.Update(User);
            else
                _userService.Insert(User);

            NavigateBack();
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            _regionManager.RequestNavigate("ContentRegion", "UserListView");
        }

        /// <summary>
        /// 导航进入时根据参数判断是新增还是编辑
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            if (navigationContext.Parameters.ContainsKey("User"))
            {
                var user = navigationContext.Parameters.GetValue<User>("User");
                // 克隆对象，避免直接修改列表中的引用导致取消后数据已变
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
            OnPropertyChanged(nameof(DisplayText));
        }
    }
}