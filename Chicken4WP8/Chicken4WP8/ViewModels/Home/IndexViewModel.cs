using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : TweetPivotItemViewModelBase
    {
        #region properties
        private IStatusController statusController;
        private IUserController userController;

        public IImageCacheService ImageCacheService { get; set; }

        public IndexViewModel(
            IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> statusControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
        {
            statusController = statusControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        #endregion

        protected override async Task RealizeItem(ITweetModel item)
        {
            if (item.User.ImageSource != null)
            {
                Debug.WriteLine("user {0} 's avatar already realized, image url is: {1}", item.User.ScreenName, item.User.ProfileImageUrl);
                return;
            }
            //get cached profile image
            var data = ImageCacheService.GetCachedProfileImage(item.User);
            if (data == null)
            {
                Debug.WriteLine("user {0} 's avatar '{1}' has not been cached, download it from internet", item.User.ScreenName, item.User.ProfileImageUrl);
                data = await userController.DownloadProfileImageAsync(item.User);
                Debug.WriteLine("add user {0} 's  avatar {1}  (data length : {2}) to cache", item.User.ScreenName, item.User.ProfileImageUrl, data.Length);
                ImageCacheService.AddProfileImageToCache(item.User, data);
            }
            Debug.WriteLine("set user {0} 's avatar {1}", item.User.ScreenName, item.User.ProfileImageUrl);
            await userController.SetProfileImageAsync(item.User, data);
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromDatabase()
        {
            return null;
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options)
        {
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets != null)
                return tweets.ToList();
            return null;
        }
    }
}
