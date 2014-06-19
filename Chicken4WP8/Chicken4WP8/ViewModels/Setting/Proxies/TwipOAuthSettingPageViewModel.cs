using Caliburn.Micro;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Setting.Proxies
{
    public class TwipOAuthSettingPageViewModel : Screen
    {
        public ILanguageHelper LanguageHelper { get; set; }

        public TwipOAuthSettingPageViewModel()
        { }

        public void Finish()
        {
        }
    }
}
