using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelineController : ITimelineController
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITimelineQueryExecutor _timelineQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFactory<IHomeTimelineRequestParameters> _homeTimelineRequestParameterFactory;
        private readonly IFactory<IUserTimelineRequestParameters> _userTimelineRequestParameterFactory;
        private readonly IFactory<IMentionsTimelineRequestParameters> _mentionsTimelineRequestParameterFactory;

        public TimelineController(
            ITweetFactory tweetFactory,
            ITimelineQueryExecutor timelineQueryExecutor,
            IUserFactory userFactory,
            IFactory<IHomeTimelineRequestParameters> homeTimelineRequestParameterFactory,
            IFactory<IUserTimelineRequestParameters> userTimelineRequestParameterFactory,
            IFactory<IMentionsTimelineRequestParameters> mentionsTimelineRequestParameterFactory)
        {
            _tweetFactory = tweetFactory;
            _timelineQueryExecutor = timelineQueryExecutor;
            _userFactory = userFactory;
            _homeTimelineRequestParameterFactory = homeTimelineRequestParameterFactory;
            _userTimelineRequestParameterFactory = userTimelineRequestParameterFactory;
            _mentionsTimelineRequestParameterFactory = mentionsTimelineRequestParameterFactory;
        }

        // Home Timeline
        public IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve)
        {
            var timelineRequestParameter = _homeTimelineRequestParameterFactory.Create();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweetsToRetrieve;
            return GetHomeTimeline(timelineRequestParameter);
        }

        public IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters)
        {
            var timelineDTO = _timelineQueryExecutor.GetHomeTimeline(timelineRequestParameters);
            return _tweetFactory.GenerateTweetsFromDTO(timelineDTO);
        }

        // User Timeline
        public IEnumerable<ITweet> GetUserTimeline(IUser user, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GetUserIdentifierFromUser(user);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40)
        {
            var requestParameters = _userTimelineRequestParameterFactory.Create();
            requestParameters.UserIdentifier = userIdentifier;
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetUserTimeline(requestParameters);
        }

        public IEnumerable<ITweet> GetUserTimeline(long userId, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters)
        {
            var tweetsDTO = _timelineQueryExecutor.GetUserTimeline(timelineRequestParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        // Mention Timeline
        public IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var timelineRequestParameter = _mentionsTimelineRequestParameterFactory.Create();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetMentionsTimeline(timelineRequestParameter);
        }

        public IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineRequestParameters mentionsTimelineRequestParameters)
        {
            var timelineDTO = _timelineQueryExecutor.GetMentionsTimeline(mentionsTimelineRequestParameters);
            return _tweetFactory.GenerateMentionsFromDTO(timelineDTO);
        }
    }
}