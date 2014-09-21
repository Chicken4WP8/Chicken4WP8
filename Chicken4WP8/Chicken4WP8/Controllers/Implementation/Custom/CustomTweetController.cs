using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken4WP8.Controllers.Implementation.Base;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implementation.Custom
{
    public class CustomTweetController : CustomControllerBase, ITweetController
    {
        public CustomTweetController()
        { }

        public async Task<IEnumerable<ITweetModel>> HomeTimelineAsync(IDictionary<string, object> parameters = null)
        {
            var tweets = await tokens.Statuses.HomeTimelineAsync(parameters);
            var list = new List<ITweetModel>();
            if (tweets != null)
                foreach (var tweet in tweets)
                    list.Add(new TweetModel(tweet));
            StorageService.AddCachedTweets(list);
            return list;
        }

        public async Task<IEnumerable<ITweetModel>> MentionsTimelineAsync(IDictionary<string, object> parameters = null)
        {
            var tweets = await tokens.Statuses.MentionsTimelineAsync(parameters);
            var list = new List<ITweetModel>();
            if (tweets != null)
                foreach (var tweet in tweets)
                    list.Add(new TweetModel(tweet));
            return list;
        }

        public async Task<ITweetModel> ShowAsync(IDictionary<string, object> parameters)
        {
            var status = await tokens.Statuses.ShowAsync(parameters);
            return new TweetModel(status);
        }

        public async Task SetStatusImagesAsync(ITweetModel status)
        {
            List<IMediaEntity> medias = null;
            if (status.RetweetedStatus != null
                && status.RetweetedStatus.Entities != null
                && status.RetweetedStatus.Entities.Media != null
                && status.RetweetedStatus.Entities.Media.Count != 0)
                medias = status.RetweetedStatus.Entities.Media;
            else if (status.Entities != null
                && status.Entities.Media != null
                && status.Entities.Media.Count != 0)
                medias = status.Entities.Media;
            if (medias != null && medias.Count != 0)
            {
                if (medias[0].MediaUrlHttps != null)
                {
                    string url = medias[0].MediaUrlHttps.AbsoluteUri;
                    medias[0].ImageData = await base.GetImageAsync(url, url);
                }
            }
        }

        public async Task<ITweetModel> UpdateAsync(IDictionary<string, object> parameters)
        {
            var status = await tokens.Statuses.UpdateAsync(parameters);
            return new TweetModel(status);
        }
    }
}
