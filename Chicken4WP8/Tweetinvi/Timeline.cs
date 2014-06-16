using System;
using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi
{
    public static class Timeline
    {
        [ThreadStatic]
        private static ITimelineController _timelineController;
        public static ITimelineController TimelineController
        {
            get
            {
                if (_timelineController == null)
                {
                    Initialize();
                }

                return _timelineController;
            }
        }

        private static IFactory<IHomeTimelineRequestParameters> _homeTimelineParameterFactory;
        private static IFactory<IUserTimelineRequestParameters> _userTimelineParameterFactory;
        private static IFactory<IMentionsTimelineRequestParameters> _mentionsTimelineParameterFactory;
        private static IUserFactory _userFactory;

        static Timeline()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _timelineController = TweetinviContainer.Resolve<ITimelineController>();
            _homeTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IHomeTimelineRequestParameters>>();
            _userTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IUserTimelineRequestParameters>>();
            _mentionsTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IMentionsTimelineRequestParameters>>();
            _userFactory = TweetinviContainer.Resolve<IUserFactory>();
        }

        // Parameter generator
        public static IHomeTimelineRequestParameters CreateHomeTimelineRequestParameter()
        {
            return _homeTimelineParameterFactory.Create();
        }

        public static IUserTimelineRequestParameters CreateUserTimelineRequestParameter(IUser user)
        {
            var userIdentifier = _userFactory.GetUserIdentifierFromUser(user);
            return CreateUserTimelineRequestParameter(userIdentifier);
        }

        public static IUserTimelineRequestParameters CreateUserTimelineRequestParameter(long userId)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return CreateUserTimelineRequestParameter(userIdentifier);
        }

        public static IUserTimelineRequestParameters CreateUserTimelineRequestParameter(string screenName)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(screenName);
            return CreateUserTimelineRequestParameter(userIdentifier);
        }

        public static IUserTimelineRequestParameters CreateUserTimelineRequestParameter(IUserIdentifier userIdentifier)
        {
            var requestParameter = _userTimelineParameterFactory.Create();
            requestParameter.UserIdentifier = userIdentifier;
            return requestParameter;
        }

        public static IMentionsTimelineRequestParameters CreateMentionsTimelineRequestParameters()
        {
            return _mentionsTimelineParameterFactory.Create();
        }

        // Home Timeline
        public static IEnumerable<ITweet> GetHomeTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetHomeTimeline(maximumTweets);
        }

        public static IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineRequestParameters homeTimelineRequestRequestParameters)
        {
            return TimelineController.GetHomeTimeline(homeTimelineRequestRequestParameters);
        }

        // User Timeline
        public static IEnumerable<ITweet> GetUserTimeline(IUser user, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(user, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userDTO, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userDTO, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userId, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userScreenName, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(IUserTimelineRequestParameters userTimelineRequestParameters)
        {
            return TimelineController.GetUserTimeline(userTimelineRequestParameters);
        }

        // Mention Timeline
        public static IEnumerable<IMention> GetMentionsTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetMentionsTimeline(maximumTweets);
        }

        public static IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineRequestParameters mentionsTimelineRequestParameters)
        {
            return TimelineController.GetMentionsTimeline(mentionsTimelineRequestParameters);
        }
    }
}