using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;
using Prism.Navigation;

namespace WpfAppDemo.Common
{
    /// <summary>
    /// 视图模型基类，集成 CommunityToolkit.Mvvm 的观测功能与 Prism 的导航功能
    /// </summary>
    public abstract partial class ViewModelBase : ObservableObject, INavigationAware, IConfirmNavigationRequest
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ViewModelBase()
        {
        }

        #region INavigationAware 接口实现

        /// <summary>
        /// 确定此视图模型是否可以处理导航请求
        /// </summary>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <summary>
        /// 导航离开当前视图时触发
        /// </summary>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 导航到当前视图时触发
        /// </summary>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 确认是否允许导航离开
        /// </summary>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, System.Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        #endregion
    }
}
