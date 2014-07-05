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
        private long? sinceId = -1, maxId = -1;
        private IStatusController statusController;

        public IndexViewModel(IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> controllers)
        {
            statusController = controllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        protected override async Task RealizeItem(ITweetModel item)
        {
            if (item.User.ImageSource != null)
            {
                Debug.WriteLine("user {0} 's avatar already realized, image url is: {1}", item.User.ScreenName, item.User.ProfileImageUrl);
                return;
            }
            Debug.WriteLine("get user {0} avatar image from internet, image url is: {1}", item.User.ScreenName, item.User.ProfileImageUrl);
            //var stream = await item.Creator.User.GetProfileImageStreamAsync();
            //base.SetImageFromStream(item.Creator, stream);
        }

        protected override async Task<IEnumerable<ITweetModel>> FetchData()
        {
            var options = TwitterHelper.GetDictionary();
            options.Add(Const.SINCE_ID, sinceId);
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets.Count() != 0)
                sinceId = tweets.First().Id;
            return tweets;
        }

        protected override async Task<IEnumerable<ITweetModel>> LoadData()
        {
            var options = TwitterHelper.GetDictionary();
            options.Add(Const.MAX_ID, maxId);
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets.Count() != 0)
                maxId = tweets.Last().Id;
            return tweets;
        }
    }
}
