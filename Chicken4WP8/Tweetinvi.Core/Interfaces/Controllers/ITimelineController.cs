using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface ITimelineController
    {
        // Home Timeline
        IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters);

        // User Timeline
        IEnumerable<ITweet> GetUserTimeline(IUser user, int maximumNumberOfTweets = 40);
        IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40);
        IEnumerable<ITweet> GetUserTimeline(long userId, int maximumNumberOfTweets = 40);
        IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40);

        IEnumerable<ITweet> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters);

        // Mention Timeline
        IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfTweets = 40);
        IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineRequestParameters mentionsTimelineRequestParameters);
    }
}