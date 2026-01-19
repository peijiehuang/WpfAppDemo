using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 消息对话框服务接口
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="titleKey">标题资源键</param>
        Task ShowMessageAsync(string message, string titleKey = "Common_Info");

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="message">询问内容</param>
        /// <param name="titleKey">标题资源键</param>
        /// <returns>用户是否点击确认</returns>
        Task<bool> ShowConfirmationAsync(string message, string titleKey = "Common_DeleteConfirm");
    }

    /// <summary>
    /// 消息对话框服务实现类，集成 MaterialDesignThemes
    /// </summary>
    public class MessageService : IMessageService
    {
        private string GetResourceString(string key, string defaultValue)
        {
            try
            {
                return Application.Current.TryFindResource(key)?.ToString() ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 异步显示消息
        /// </summary>
        public async Task ShowMessageAsync(string message, string titleKey = "Common_Info")
        {
            string title = GetResourceString(titleKey, "提示");
            var dialog = new Views.CommonDialogView(title, message, false);
            await DialogHost.Show(dialog, "RootDialog");
        }

        /// <summary>
        /// 异步显示确认框
        /// </summary>
        public async Task<bool> ShowConfirmationAsync(string message, string titleKey = "Common_DeleteConfirm")
        {
            string title = GetResourceString(titleKey, "确认");
            
            // 尝试转换内置消息
            if (message == "确定要删除这条记录吗?")
            {
                message = GetResourceString("Common_DeleteMessage", message);
            }

            var dialog = new Views.CommonDialogView(title, message, true);
            var result = await DialogHost.Show(dialog, "RootDialog");
            return result is bool b && b;
        }
    }
}