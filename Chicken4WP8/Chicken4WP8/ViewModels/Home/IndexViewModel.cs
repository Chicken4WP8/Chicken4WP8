using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<TweetModel>
    {
        protected override async Task RealizeItem(TweetModel item)
        {
            if (string.IsNullOrEmpty(item.Creator.ProfileImageUrl)
                || item.Creator.ProfileImage != null)
            {
                Debug.WriteLine("user {0} 's avatar already realized, image url is: {1}", item.Creator.ScreenName, item.Creator.ProfileImageUrl);
                return;
            }
            Debug.WriteLine("get user {0} avatar image from internet, image url is: {1}", item.Creator.ScreenName, item.Creator.ProfileImageUrl);
            var stream = await item.Creator.User.GetProfileImageStreamAsync();
            base.SetImageFromStream(item.Creator, stream);
        }

        protected override async Task<IEnumerable<TweetModel>> FetchData()
        {
            var loggedUser = App.LoggedUser;
            var list = new List<TweetModel>();
            var tweets = await loggedUser.GetHomeTimelineAsync();
            if (tweets != null)
                foreach (var tweet in tweets)
                    list.Add(new TweetModel(tweet));
            return list;
        }
    }
}
