using System;
using System.Linq;
using System.Windows;

namespace WpfAppDemo.Services
{
    public interface ILocalizationService
    {
        string CurrentLanguage { get; }
        event Action LanguageChanged;
        void SetLanguage(string languageCode);
    }

    public class LocalizationService : ILocalizationService
    {
        public string CurrentLanguage { get; private set; } = "zh-CN";
        public event Action LanguageChanged = delegate { };

        public void SetLanguage(string languageCode)
        {
            var app = Application.Current;
            var dictionaries = app.Resources.MergedDictionaries;

            // Use a more robust way to find the language dictionary
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
