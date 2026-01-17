using MaterialDesignThemes.Wpf;

namespace WpfAppDemo.Services
{
    public interface IThemeService
    {
        bool IsDarkMode { get; }
        void SetTheme(bool isDark);
        void ToggleTheme();
    }

    public class ThemeService : IThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public bool IsDarkMode 
        {
            get
            {
                var theme = _paletteHelper.GetTheme();
                return theme.GetBaseTheme() == BaseTheme.Dark;
            }
        }

        public void SetTheme(bool isDark)
        {
            var theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            _paletteHelper.SetTheme(theme);
        }

        public void ToggleTheme()
        {
            SetTheme(!IsDarkMode);
        }
    }
}
