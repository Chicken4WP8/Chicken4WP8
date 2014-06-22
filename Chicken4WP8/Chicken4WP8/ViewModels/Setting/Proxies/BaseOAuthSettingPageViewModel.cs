using System;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.Views.Setting.Proxies;
using Tweetinvi;

namespace Chicken4WP8.ViewModels.Setting.Proxies
{
    public class BaseOAuthSettingPageViewModel : Screen
    {
        public ILanguageHelper LanguageHelper { get; set; }

        public BaseOAuthSettingPageViewModel()
        {
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var credentials = CredentialsCreator.GenerateApplicationCredentials("pPnxpn00RbGx3YJJtvYUsA", "PoX3exts23HJ1rlMaPr6RtlX2G5VQdrqbpUWpkMcCo");
            var url = await CredentialsCreator.GetAuthorizationURLAsync(credentials);

            var page = view as BaseOAuthSettingPageView;
            var browser = page.Browser;
            browser.Navigate(new Uri(url, UriKind.Absolute));
        }

        private string pin;
        public string PinCode
        {
            get { return pin; }
            set
            {
                pin = value;
                NotifyOfPropertyChange(() => PinCode);
            }
        }

        public void AppBar_Finish()
        {
        }
    }
}
