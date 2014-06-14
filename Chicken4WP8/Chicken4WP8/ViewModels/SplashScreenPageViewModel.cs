using Caliburn.Micro;
using Chicken4WP8.Services.Interface;

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
            var credential = StorageService.GetCurrentOAuthSetting();
            if (credential.HasAllCredentials())
            {
                SharedObjects.Credential = credential;
            }
            else
            {
 
            }
        }
    }
}
