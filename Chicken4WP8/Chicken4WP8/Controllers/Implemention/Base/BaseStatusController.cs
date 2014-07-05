using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseStatusController : BaseControllerBase, IStatusController
    {
        public BaseStatusController()
        {
        }

        public async Task<IEnumerable<ITweetModel>> HomeTimelineAsync(IDictionary<string, object> parameters = null)
        {
            var tweets = await tokens.Statuses.HomeTimelineAsync(parameters);
            var list = new List<ITweetModel>(tweets.Count);
            if (tweets != null)
                foreach (var tweet in tweets)
                    list.Add(new TweetModel(tweet));
            return list;
        }
    }
}
