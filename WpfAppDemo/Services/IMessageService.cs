using System;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace WpfAppDemo.Services
{
    public interface IMessageService
    {
        Task ShowMessageAsync(string message, string titleKey = "Common_Info");
        Task<bool> ShowConfirmationAsync(string message, string titleKey = "Common_DeleteConfirm");
    }

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

        public async Task ShowMessageAsync(string message, string titleKey = "Common_Info")
        {
            string title = GetResourceString(titleKey, "Information");
            var dialog = new Views.CommonDialogView(title, message, false);
            await DialogHost.Show(dialog, "RootDialog");
        }

        public async Task<bool> ShowConfirmationAsync(string message, string titleKey = "Common_DeleteConfirm")
        {
            string title = GetResourceString(titleKey, "Confirmation");
            
            // 如果传入的是硬编码的中文字符串，尝试转换成资源中的描述
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
