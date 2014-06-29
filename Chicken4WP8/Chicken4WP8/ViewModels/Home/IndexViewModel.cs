using System.Threading.Tasks;
using System.Windows.Media;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<TweetModel>
    {
        protected async override Task ItemRealized(TweetModel item)
        {
            if (string.IsNullOrEmpty(item.Creator.ProfileImageUrl)
                || item.Creator.ProfileImage != null)
                return;
            var stream = await item.Creator.User.GetProfileImageStreamAsync();
            base.SetImageFromStream(item.Creator.ProfileImage, stream);
        }

        protected override async Task FetchData()
        {
            var loggedUser = App.LoggedUser;
            var tweets = await loggedUser.GetHomeTimelineAsync();

            foreach (var tweet in tweets)
                Items.Add(new TweetModel(tweet));
        }
    }
}
