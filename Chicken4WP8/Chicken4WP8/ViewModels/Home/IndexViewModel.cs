using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<ITweetModel>
    {
        private long? sinceId, maxId;
        private IStatusController statusController;
        private IUserController userController;

        public IndexViewModel(
            IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> statusControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
        {
            statusController = statusControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        protected override async Task RealizeItem(ITweetModel item)
        {
            if (item.User.ImageSource != null)
            {
                Debug.WriteLine("user {0} 's avatar already realized, image url is: {1}", item.User.ScreenName, item.User.ProfileImageUrl);
                return;
            }
            Debug.WriteLine("get user {0} avatar image from internet, image url is: {1}", item.User.ScreenName, item.User.ProfileImageUrl);
            await userController.SetProfileImageStreamAsync(item.User);
        }

        protected override async Task<IEnumerable<ITweetModel>> FetchData()
        {
            var options = TwitterHelper.GetDictionary();
            if (sinceId != null)
                options.Add(Const.SINCE_ID, sinceId);
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets.Count() != 0)
                sinceId = tweets.First().Id+1;
            return tweets;
        }

        protected override async Task<IEnumerable<ITweetModel>> LoadData()
        {
            var options = TwitterHelper.GetDictionary();
            if (maxId != null)
                options.Add(Const.MAX_ID, maxId);
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets.Count() != 0)
                maxId = tweets.Last().Id+1;
            return tweets;
        }
    }
}
