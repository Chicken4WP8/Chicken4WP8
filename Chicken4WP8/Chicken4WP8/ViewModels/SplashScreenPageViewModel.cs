﻿using System;
using Caliburn.Micro;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Home;
using Chicken4WP8.ViewModels.Setting;

namespace Chicken4WP8.ViewModels
{
    public class SplashScreenPageViewModel : Screen
    {
        #region properties
        public IStorageService StorageService { get; set; }
        public INavigationService NavigationService { get; set; }
        public IToastMessageService ToastMessageService { get; set; }

        public SplashScreenPageViewModel()
        { }
        #endregion

        protected async override void OnInitialize()
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
                var type = setting.OAuthSetting.GetType();
                if (type == typeof(BaseOAuthSetting))
                {
                    //set oauth key and secret for access
                    //TwitterCredentials.SetCredentials(setting.OAuthSetting as IOAuthCredentials);
                }
                else if (type == typeof(TwipOAuthSetting))
                {
                    //set base url for twip
                }
                try
                {
                    //var user = null;// await User.GetLoggedUserAsync();
                    //if (user != null)
                    //{
                    //    App.UpdateLoggedUser(user);
                    //    NavigationService.UriFor<HomePageViewModel>().Navigate();
                    //}
                }
                catch (Exception e)
                {
                    ToastMessageService.HandleMessage(e.Message);
                }
            }
        }
    }
}
