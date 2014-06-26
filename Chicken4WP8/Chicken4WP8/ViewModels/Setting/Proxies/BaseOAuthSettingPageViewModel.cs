using System;
using Caliburn.Micro;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.Views.Setting.Proxies;
using Microsoft.Phone.Controls;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Chicken4WP8.ViewModels.Setting.Proxies
{
    public class BaseOAuthSettingPageViewModel : Screen
    {
        #region properties
        private const string KEY = "pPnxpn00RbGx3YJJtvYUsA";
        private const string SECRET = "PoX3exts23HJ1rlMaPr6RtlX2G5VQdrqbpUWpkMcCo";
        private ITemporaryCredentials credentials;
        private readonly WaitCursor waitCursorService;
        public IStorageService StorageService { get; set; }
        public INavigationService NavigationService { get; set; }

        public ILanguageHelper LanguageHelper { get; set; }

        public BaseOAuthSettingPageViewModel()
        {
            waitCursorService = WaitCursorService.WaitCursor;
        }
        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();
            waitCursorService.Text = LanguageHelper["WaitCursor_Loading"];
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            credentials = CredentialsCreator.GenerateApplicationCredentials(KEY, SECRET);
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

        public async void AppBar_Finish()
        {
            if (string.IsNullOrEmpty(PinCode))
                return;
            waitCursorService.IsVisible = true;
            var newCredentials = await CredentialsCreator.GetCredentialsFromVerifierCodeAsync(PinCode, credentials);
            TwitterCredentials.SetCredentials(newCredentials);
            var oauth = new BaseOAuthSetting
            {
                ConsumerKey = newCredentials.ConsumerKey,
                ConsumerSecret = newCredentials.ConsumerSecret,
                AccessToken = newCredentials.AccessToken,
                AccessTokenSecret = newCredentials.AccessTokenSecret
            };

            var user = await User.GetLoggedUserAsync();

            var setting = StorageService.GetCurrentUserSetting();
            if (setting == null)
                setting = new UserSetting();
            setting.OAuthSetting = oauth;
            setting.Id = user.Id;
            setting.Name = user.Name;
            setting.ScreenName = user.ScreenName;

            StorageService.UpdateCurrentUserSetting(setting);
            App.UpdateSetting(setting);
            waitCursorService.IsVisible = false;
        }
    }
}
