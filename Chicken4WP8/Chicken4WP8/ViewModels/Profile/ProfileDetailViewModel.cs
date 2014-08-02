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
    public class ProfileDetailViewModel : PivotItemViewModelBase<IUserModel>, IHandle<IUserModel>
    {
        #region properties
        protected IUserController userController;

        public ProfileDetailViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<IUserModel>();
        }
        #endregion

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["ProfileDetailViewModel_Header"];
        }

        public void Handle(IUserModel message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Items.Clear();
                Items.Add(message);
            });
        }

        protected override Task RealizeItem(IUserModel item)
        {
            Task.Factory.StartNew(() => userController.SetProfileImageAsync(item));
            Task.Factory.StartNew(() => userController.SetProfileBannerImageAsync(item));
            if (item.Id != App.UserSetting.Id)
                Task.Factory.StartNew(() => userController.LookupFriendshipAsync(item));
            return Task.Delay(0);
        }

        protected override async Task FetchMoreDataFromWeb()
        {
            if (Items.Count == 0)
                return;
            var option = Const.GetDictionary();
            option.Add(Const.USER_ID, Items[0].Id);
            option.Add(Const.USER_SCREEN_NAME, Items[0].ScreenName);
            option.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            var profile = await userController.ShowAsync(option);
            if (profile != null)
            {
                profile.IsProfileDetail = true;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Items.Clear();
                        Items.Add(profile);
                    });
            }
        }

        protected override Task LoadDataFromWeb()
        {
            return Task.Delay(0);
        }
    }
}