using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Home;
using Chicken4WP8.ViewModels.Setting;
using Chicken4WP8.ViewModels.Status;

namespace Chicken4WP8.ViewModels
{
    public class SplashScreenPageViewModel : Screen
    {
        #region properties
        public IStorageService StorageService { get; set; }
        public INavigationService NavigationService { get; set; }

        public SplashScreenPageViewModel()
        { }
        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var setting = StorageService.GetCurrentUserSetting();
            if (setting == null || setting.OAuthSetting == null)
            {
                //goto oauth setting page
                NavigationService.UriFor<OAuthSettingPageViewModel>().Navigate();
            }
            else
            {
                App.UpdateSetting(setting);
                //NavigationService.UriFor<HomePageViewModel>().Navigate();
                NavigationService.UriFor<NewStatusPageViewModel>().Navigate();
            }
        }
    }
}
