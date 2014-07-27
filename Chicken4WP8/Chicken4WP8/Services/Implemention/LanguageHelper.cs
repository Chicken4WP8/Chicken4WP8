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
        private bool isInit;

        public IEventAggregator EventAggregator { get; set; }
        public IStorageService StorageService { get; set; }

        public LanguageHelper()
        { }

        public void SetLanguage(CultureInfo cultureInfo)
        {
            StorageService.UpdateLanguage(cultureInfo.Name);
            isInit = false;
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

        private string GetString(string key)
        {
            if (!isInit)
                InitCurrentCulture();

            return AppResources.ResourceManager.GetString(key, AppResources.Culture);
        }

        private void InitCurrentCulture()
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var current = StorageService.GetCurrentLanguage();
            if (!string.IsNullOrEmpty(current))
                cultureInfo = new CultureInfo(current);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            AppResources.Culture = cultureInfo;
            isInit = true;
        }
    }
}
