using Prism.Mvvm;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 忙碌状态服务接口
    /// </summary>
    public interface IBusyService
    {
        /// <summary>
        /// 是否处于忙碌状态
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// 设置为忙碌状态
        /// </summary>
        /// <param name="message">提示消息</param>
        void Busy(string? message = null);

        /// <summary>
        /// 设置为空闲状态
        /// </summary>
        void Idle();
    }

    /// <summary>
    /// 忙碌状态服务实现类
    /// </summary>
    public class BusyService : BindableBase, IBusyService
    {
        private bool _isBusy;
        private string? _message;

        /// <summary>
        /// 是否忙碌
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string? Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// 进入忙碌状态
        /// </summary>
        public void Busy(string? message = null)
        {
            Message = message;
            IsBusy = true;
        }

        /// <summary>
        /// 恢复空闲状态
        /// </summary>
        public void Idle()
        {
            IsBusy = false;
            Message = null;
        }
    }
}