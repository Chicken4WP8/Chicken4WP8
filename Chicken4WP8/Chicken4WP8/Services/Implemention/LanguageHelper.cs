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
        public IEventAggregator EventAggregator { get; set; }
        public IStorageService StorageService { get; set; }

        public LanguageHelper()
        {
            var cultureInfo = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            AppResources.Culture = cultureInfo;
        }

        public void SetLanguage(CultureInfo cultureInfo)
        {
            StorageService.UpdateLanguage(cultureInfo.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            NotifyOfPropertyChange("Item[]");
            EventAggregator.Publish(cultureInfo, action => Task.Factory.StartNew(action));
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
            return AppResources.ResourceManager.GetString(key,AppResources.Culture);
        }
    }
}
