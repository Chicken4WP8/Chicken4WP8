using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Home;
using Chicken4WP8.ViewModels.Setting;

namespace Chicken4WP8.ViewModels
{
    public class SplashScreenPageViewModel : Screen
    {
        private readonly IStorageService storageService;
        private readonly INavigationService navigationService;

        public SplashScreenPageViewModel(
            IStorageService storageService,
            INavigationService navigationService)
        {
            this.storageService = storageService;
            this.navigationService = navigationService;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var setting = storageService.GetCurrentUserSetting();
            if (setting == null)
            {
                //goto oauth setting page
                navigationService.UriFor<OAuthSettingPageViewModel>().Navigate();
            }
            else
            {
                //set base url for twip or
                //set oauth key
                navigationService.UriFor<HomePageViewModel>().Navigate();
            }
        }
    }
}
