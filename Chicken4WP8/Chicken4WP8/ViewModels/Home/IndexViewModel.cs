using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<TweetModel>
    {
        protected async Task ItemRealized(TweetModel item)
        {
            if (string.IsNullOrEmpty(item.Creator.ProfileImageUrl)
                || item.Creator.ProfileImage != null)
                return;
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
