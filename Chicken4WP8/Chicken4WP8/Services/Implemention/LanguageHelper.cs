using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Resources;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Services.Implemention
{
    public class LanguageHelper : PropertyChangedBase, ILanguageHelper
    {
        public IEventAggregator Aggregator { get; set; }
        public IStorageService StorageService { get; set; }

        public LanguageHelper()
        { }

        public void SetLanguage(CultureInfo cultureInfo)
        {
            StorageService.UpdateLanguage(cultureInfo.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            NotifyOfPropertyChange("Item[]");
            Aggregator.Publish(cultureInfo, action => Task.Factory.StartNew(action));
        }

        public string this[string key]
        {
            get { return GetString(key); }
        }

        public string GetString(string key, params string[] parameters)
        {
            return string.Format(GetString(key), parameters);
        }

        private static string GetString(string key)
        {
            return AppResources.ResourceManager.GetString(key);
        }
    }
}
