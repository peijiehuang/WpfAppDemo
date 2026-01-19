namespace WpfAppDemo.Models
{
    /// <summary>
    /// 主菜单项模型
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 标题资源键
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// MaterialDesign 图标名称
        /// </summary>
        public string Icon { get; }

        /// <summary>
        /// 导航路径（View 名称）
        /// </summary>
        public string NavigationPath { get; }

        public MenuItem(string title, string icon, string navigationPath)
        {
            Title = title;
            Icon = icon;
            NavigationPath = navigationPath;
        }
    }
}
