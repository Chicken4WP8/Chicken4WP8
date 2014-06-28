using System;
using Caliburn.Micro;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Home;
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
        public IToastMessageService ToastMessageService { get; set; }

        public BaseOAuthSettingPageViewModel()
        {
            waitCursorService = WaitCursorService.WaitCursor;
        }
        #endregion

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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            waitCursorService.Text = LanguageHelper["WaitCursor_Loading"];
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            waitCursorService.Text = LanguageHelper["WaitCursor_GetAuthorizationPage"];
            waitCursorService.IsVisible = true;

            credentials = CredentialsCreator.GenerateApplicationCredentials(KEY, SECRET);

            var page = view as BaseOAuthSettingPageView;
            var browser = page.Browser;
            browser.Navigated += (o, e) =>
            {
                waitCursorService.IsVisible = false;
            };
            browser.NavigationFailed += (o, e) => BrowserNavigationFailed(e.Exception);

            try
            {
                var url = await CredentialsCreator.GetAuthorizationURLAsync(credentials);
                browser.Navigate(new Uri(url, UriKind.Absolute));
            }
            catch (Exception e)
            {
                BrowserNavigationFailed(e);
            }
        }

        public async void AppBar_Finish()
        {
            if (string.IsNullOrEmpty(PinCode))
                return;
            waitCursorService.IsVisible = true;
            waitCursorService.Text = LanguageHelper["WaitCursor_GetCredentials"];

            var setting = StorageService.GetCurrentUserSetting();
            if (setting == null)
                setting = new UserSetting();

            var newCredentials = await CredentialsCreator.GetCredentialsFromVerifierCodeAsync(PinCode, credentials);
            TwitterCredentials.SetCredentials(newCredentials);
            var oauth = new BaseOAuthSetting
            {
                ConsumerKey = newCredentials.ConsumerKey,
                ConsumerSecret = newCredentials.ConsumerSecret,
                AccessToken = newCredentials.AccessToken,
                AccessTokenSecret = newCredentials.AccessTokenSecret
            };

            waitCursorService.Text = LanguageHelper["WaitCursor_GetCurrentUser"];
            var user = await User.GetLoggedUserAsync();

            setting.OAuthSetting = oauth;
            setting.LoggedUser = user;

            StorageService.UpdateCurrentUserSetting(setting);
            App.UpdateSetting(setting);
            waitCursorService.IsVisible = false;

            ToastMessageService.HandleMessage(
                LanguageHelper.GetString("Toast_Msg_HelloUser", user.ScreenName),
                () =>
                    NavigationService.UriFor<HomePageViewModel>().Navigate());
        }

        private async void BrowserNavigationFailed(Exception exception)
        {
            waitCursorService.IsVisible = false;

            var messageBox = new CustomMessageBox
            {
                Caption = LanguageHelper["WaitCursor_AnErrorHappened"],
                Message = exception.Message,
                LeftButtonContent = LanguageHelper["Button_OK"],
                RightButtonContent = LanguageHelper["Button_Cancel"],
                IsFullScreen = false
            };
            await messageBox.ShowAsync();
        }
    }
}
