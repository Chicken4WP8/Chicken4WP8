﻿namespace Tweetinvi.Core.Interfaces.Models.Parameters
{
    public interface IUserTimelineRequestParameters : ITimelineRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        
        /// <summary>
        /// Include Retweets. When this parameter is set to false, Twitter will send you the same result set but without including the retweets.
        /// It means that if there are a total of 100 tweets, and the latest are 80 new tweets and 20 retweets. 
        /// If the MaximumResultSet is set to 100, you will receive 80 tweets and not 100 even if there is more than 80 new tweets in the Timeline.
        /// </summary>
        bool IncludeRTS { get; set; }

        bool ExcludeReplies { get; set; }
    }
}