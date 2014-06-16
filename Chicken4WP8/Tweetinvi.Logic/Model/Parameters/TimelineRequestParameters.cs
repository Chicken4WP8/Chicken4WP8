using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Logic.Model.Parameters
{
    public class TimelineRequestParameters : ITimelineRequestParameters
    {
        public TimelineRequestParameters()
        {
            MaximumNumberOfTweetsToRetrieve = 40;

            SinceId = TweetinviConfig.DEFAULT_ID;
            MaxId = TweetinviConfig.DEFAULT_ID;

            TrimUser = false;
            IncludeContributorDetails = false;
            IncludeEntities = true;
        }

        public int MaximumNumberOfTweetsToRetrieve { get; set; }
        public long SinceId { get; set; }
        public long MaxId { get; set; }
        public bool TrimUser { get; set; }
        public bool IncludeContributorDetails { get; set; }
        public bool IncludeEntities { get; set; }
    }
}