using System;
using System.Linq;
using System.Windows;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 多语言本地化服务接口
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// 当前语言代码
        /// </summary>
        string CurrentLanguage { get; }

        /// <summary>
        /// 语言变更事件
        /// </summary>
        event Action LanguageChanged;

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="languageCode">语言代码（如 zh-CN, en-US）</param>
        void SetLanguage(string languageCode);
    }

    /// <summary>
    /// 多语言本地化服务实现类
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        public string CurrentLanguage { get; private set; } = "zh-CN";
        public event Action LanguageChanged = delegate { };

        public void SetLanguage(string languageCode)
        {
            var app = Application.Current;
            var dictionaries = app.Resources.MergedDictionaries;

            // 查找现有的语言资源字典
            var oldDict = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.ToString().Contains("Language."));
            
            var newSource = new Uri($"pack://application:,,,/Resources/Language.{languageCode}.xaml", UriKind.Absolute);
            var newDict = new ResourceDictionary { Source = newSource };

            if (oldDict != null)
            {
                int index = dictionaries.IndexOf(oldDict);
                dictionaries[index] = newDict;
            }
            else
            {
                dictionaries.Add(newDict);
            }

            CurrentLanguage = languageCode;
            LanguageChanged?.Invoke();
        }
    }
}