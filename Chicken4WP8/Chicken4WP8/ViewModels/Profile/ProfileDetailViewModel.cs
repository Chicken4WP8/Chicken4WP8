using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfileDetailViewModel : ProfileViewModelBase<IUserModel>
    {
        #region properties
        public ProfileDetailViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper, userControllers)
        { }

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

        protected override void SetUserProfile()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Items.Clear();
                Items.Add(User);
            });
        }

        protected override Task RealizeItem(IUserModel item)
        {
            Task.Factory.StartNew(() => item.IsProfileDetail ? userController.SetProfileImageDetailAsync(item) : userController.SetProfileImageAsync(item));
            Task.Factory.StartNew(() => userController.SetProfileBannerImageAsync(item));
            if (item.Id != App.UserSetting.Id)
                Task.Factory.StartNew(() => userController.LookupFriendshipAsync(item));
            listbox.ScrollTo(item);
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