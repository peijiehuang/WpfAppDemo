using System.Windows;
using System.Windows.Controls;

namespace WpfAppDemo.Views
{
    /// <summary>
    /// 通用对话框交互逻辑，用于显示提示或确认消息
    /// </summary>
    public partial class CommonDialogView : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="message">显示内容</param>
        /// <param name="showCancel">是否显示取消按钮</param>
        public CommonDialogView(string title, string message, bool showCancel)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            BtnCancel.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}