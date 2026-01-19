using System.Windows;
using WpfAppDemo.ViewModels;

namespace WpfAppDemo.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is LoginViewModel vm)
                {
                    vm.Password = PasswordBox.Password;
                }
            };
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
