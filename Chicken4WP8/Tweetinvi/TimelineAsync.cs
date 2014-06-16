﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi
{
    public static class TimelineAsync
    {
        // Home Timeline
        public static async Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(timelineRequestParameters));
        }

        // User Timeline
        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUser user, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier userDTO, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userDTO, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userId, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userScreenName, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(timelineRequestParameters));
        }

        // Mention Timeline
        public static async Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(maximumTweets));
        }

        public static async Task<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineRequestParameters timelineRequestParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(timelineRequestParameters));
        }
    }
}
