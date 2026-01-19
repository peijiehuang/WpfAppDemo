using System.Windows;
using System.Windows.Controls;

namespace WpfAppDemo.Views
{
    public partial class CommonDialogView : UserControl
    {
        public CommonDialogView(string title, string message, bool showCancel)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            BtnCancel.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
