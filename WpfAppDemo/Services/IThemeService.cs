using MaterialDesignThemes.Wpf;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 主题切换服务接口
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// 是否为深色模式
        /// </summary>
        bool IsDarkMode { get; }

        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="isDark">是否深色</param>
        void SetTheme(bool isDark);

        /// <summary>
        /// 切换主题
        /// </summary>
        void ToggleTheme();
    }

    /// <summary>
    /// 主题切换服务实现类
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        /// <summary>
        /// 获取当前是否为深色模式
        /// </summary>
        public bool IsDarkMode 
        {
            get
            {
                var theme = _paletteHelper.GetTheme();
                return theme.GetBaseTheme() == BaseTheme.Dark;
            }
        }

        /// <summary>
        /// 设置主题颜色
        /// </summary>
        public void SetTheme(bool isDark)
        {
            var theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            _paletteHelper.SetTheme(theme);
        }

        /// <summary>
        /// 切换明暗主题
        /// </summary>
        public void ToggleTheme()
        {
            SetTheme(!IsDarkMode);
        }
    }
}