using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfAppDemo.Models
{
    /// <summary>
    /// 代码生成器生成的文件模型
    /// </summary>
    public partial class GeneratedFile : ObservableObject
    {
        /// <summary>
        /// 文件显示名称（如 Service, Model）
        /// </summary>
        [ObservableProperty]
        private string _name = string.Empty;

        /// <summary>
        /// 文件的相对存储路径
        /// </summary>
        [ObservableProperty]
        private string _path = string.Empty;

        /// <summary>
        /// 文件源码内容
        /// </summary>
        [ObservableProperty]
        private string _content = string.Empty;
    }
}
