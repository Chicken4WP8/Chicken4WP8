using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Chicken4WP8.Controllers;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<ITweetModel>
    {
        private long sinceId = -1, maxId = -1;

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
            //var loggedUser = App.LoggedUser;
            var list = new List<ITweetModel>();
            //var option = new TimelineRequestParameters()
            //{
            //    SinceId = sinceId,
            //    MaxId = maxId,
            //};
            //var tweets = await loggedUser.GetHomeTimelineAsync();
            //if (tweets != null)
            //    foreach (var tweet in tweets)
            //        list.Add(new TweetModel(tweet));
            //if (list.Count != 0)
            //    sinceId = list[0].Id;
            return list;
        }

        protected override async Task<IEnumerable<ITweetModel>> LoadData()
        {
            //var loggedUser = App.LoggedUser;
            var list = new List<ITweetModel>();
            //var tweets = await App.Tokens.Statuses.HomeTimelineAsync();
            //var option = new TimelineRequestParameters()
            //{
            //    SinceId = sinceId,
            //    MaxId = maxId,
            //};
            //var tweets = await loggedUser.GetHomeTimelineAsync();
            //if (tweets != null)
            //foreach (var tweet in tweets)
            //    list.Add(new TweetModel(tweet));
            //if (list.Count != 0)
            //    maxId = list[list.Count-1].Id;
            return list;
        }
    }
}
