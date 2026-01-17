using System.Threading.Tasks;

namespace WpfAppDemo.Services
{
    public interface IMessageService
    {
        Task ShowMessageAsync(string message, string title = "Information");
        Task<bool> ShowConfirmationAsync(string message, string title = "Confirmation");
    }

    public class MessageService : IMessageService
    {
        public Task ShowMessageAsync(string message, string title = "Information")
        {
            // Simple wrapper for now, could integrate with MaterialDesign DialogHost
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            return Task.CompletedTask;
        }

        public Task<bool> ShowConfirmationAsync(string message, string title = "Confirmation")
        {
            var result = System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            return Task.FromResult(result == System.Windows.MessageBoxResult.Yes);
        }
    }
}
