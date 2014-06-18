using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Home;

namespace Chicken4WP8.ViewModels
{
    public class SplashScreenPageViewModel : Screen
    {
        public IStorageService StorageService { get; set; }
        public INavigationService NavigationService { get; set; }

        public SplashScreenPageViewModel()
        { }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var setting = StorageService.GetCurrentUserSetting();
            if (setting == null)
            {
                //setting page
            }
            else
            {
                //set base url for twip or
                //set oauth key
                NavigationService.UriFor<HomePageViewModel>().Navigate();
            }
        }
    }
}
