using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Tweetinvi;

namespace Chicken4WP8.ViewModels.Setting.Proxies
{
    public class TwipOAuthSettingPageViewModel : Screen
    {
        public ILanguageHelper LanguageHelper { get; set; }

        public TwipOAuthSettingPageViewModel()
        { }

        private string baseUrl;
        public string BaseUrl
        {
            get { return baseUrl; }
            set
            {
                baseUrl = value;
                NotifyOfPropertyChange(() => BaseUrl);
            }
        }

        public void Finish()
        {
            if (!string.IsNullOrEmpty(BaseUrl))
            {
                TwitterResources.BaseUrl = BaseUrl;
            }
        }
    }
}
