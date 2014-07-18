using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Status
{
    public class StatusDetailViewModel : PivotItemViewModelBase<ITweetModel>
    {
        #region properties
        protected IStatusController statusController;
        protected IUserController userController;

        public StatusDetailViewModel(
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> statusControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(languageHelper)
        {
            statusController = statusControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }
        #endregion

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<ITweetModel>();

            await ShowProgressBar();
            //initialize the tweet from cache
            var tweet = StorageService.GetTempTweet();
            Items.Add(tweet);
            await HideProgressBar();
        }

        protected override async Task RealizeItem(ITweetModel item)
        {
            var user = item.RetweetedStatus == null ? item.User : item.RetweetedStatus.User;
            if (user.ImageSource != null)
                return;
            var data = StorageService.GetCachedImage(user.Id.Value + user.ProfileImageUrl);
            if (data == null)
            {
                data = await userController.DownloadProfileImageAsync(user);
                StorageService.AddOrUpdateImageCache(user.Id.Value + user.ProfileImageUrl, data);
            }
            await userController.SetProfileImageAsync(user, data);
        }

        protected override Task FetchMoreDataFromWeb()
        {
            return null;
        }

        protected override Task LoadMoreDataFromWeb()
        {
            return null;
        }
    }
}
