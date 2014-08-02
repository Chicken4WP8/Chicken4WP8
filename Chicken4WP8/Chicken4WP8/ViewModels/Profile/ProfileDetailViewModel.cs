using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfileDetailViewModel : PivotItemViewModelBase<IUserModel>
    {
        #region properties
        private ProfilePageNavigationArgs args;
        protected IUserController userController;

        public ProfileDetailViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        #endregion

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<IUserModel>();
            Items.Clear();

            await ShowProgressBar();
            //initialize the user from cache
            args = StorageService.GetTempUser();
            if (args.Mention != null)
            {
                await Task.Factory.StartNew(() => FetchMoreDataFromWeb());
            }
            else
            {
                args.User.IsProfileDetail = true;
                Items.Add(args.User);
            }
            await HideProgressBar();
        }

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["ProfileDetailViewModel_Header"];
        }

        protected async override Task RealizeItem(IUserModel item)
        {
            Task.Factory.StartNew(() => userController.SetProfileImageAsync(args.User));
            Task.Factory.StartNew(() => userController.SetProfileBannerImageAsync(args.User));
            if (args.User.Id != App.UserSetting.Id)
                Task.Factory.StartNew(() => userController.LookupFriendshipAsync(args.User));
        }

        protected override async Task FetchMoreDataFromWeb()
        {
            var option = Const.GetDictionary();
            if (args.User != null)
            {
                option.Add(Const.USER_ID, args.User.Id);
                option.Add(Const.USER_SCREEN_NAME, args.User.ScreenName);
            }
            else if (args.Mention != null)
            {
                if (args.Mention.Id != 0)
                    option.Add(Const.USER_ID, args.Mention.Id);
                if (!string.IsNullOrEmpty(args.Mention.ScreenName))
                    option.Add(Const.USER_SCREEN_NAME, args.Mention.ScreenName);
            }
            option.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            var profile = await userController.ShowAsync(option);
            if (profile != null)
            {
                profile.IsProfileDetail = true;
                args.User = profile;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Items.Clear();
                        Items.Add(args.User);
                    });
            }
        }

        protected async override Task LoadDataFromWeb()
        {
            return;
        }
    }
}