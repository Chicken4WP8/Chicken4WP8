using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineJsonController
    {
        // Home Timeline
        string GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        string GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters);

        // User Timeline
        string GetUserTimeline(IUser user, int maximumTweets = 40);
        string GetUserTimeline(IUserIdentifier userDTO, int maximumTweets = 40);
        string GetUserTimeline(long userId, int maximumTweets = 40);
        string GetUserTimeline(string userScreenName, int maximumTweets = 40);

        string GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters);

        // Mention Timeline
        string GetMentionsTimeline(int maximumNumberOfTweets = 40);
        string GetMentionsTimeline(IMentionsTimelineRequestParameters timelineRequestParameters);
    }

    public class TimelineJsonController : ITimelineJsonController
    {
        private readonly ITimelineQueryGenerator _timelineQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUserFactory _userFactory;
        private readonly IFactory<IHomeTimelineRequestParameters> _homeTimelineRequestParameterFactory;
        private readonly IFactory<IUserTimelineRequestParameters> _userTimelineRequestParameterFactory;
        private readonly IFactory<IMentionsTimelineRequestParameters> _mentionsTimelineRequestParameterFactory;

        public TimelineJsonController(
            ITimelineQueryGenerator timelineQueryGenerator,
            ITwitterAccessor twitterAccessor,
            IUserFactory userFactory,
            IFactory<IHomeTimelineRequestParameters> homeTimelineRequestParameterFactory,
            IFactory<IUserTimelineRequestParameters> userTimelineRequestParameterFactory,
            IFactory<IMentionsTimelineRequestParameters> mentionsTimelineRequestParameterFactory)
        {
            _timelineQueryGenerator = timelineQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _userFactory = userFactory;
            _homeTimelineRequestParameterFactory = homeTimelineRequestParameterFactory;
            _userTimelineRequestParameterFactory = userTimelineRequestParameterFactory;
            _mentionsTimelineRequestParameterFactory = mentionsTimelineRequestParameterFactory;
        }

        // Home Timeline
        public string GetHomeTimeline(int maximumNumberOfTweetsToRetrieve)
        {
            var timelineRequestParameter = _homeTimelineRequestParameterFactory.Create();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweetsToRetrieve;
            return GetHomeTimeline(timelineRequestParameter);
        }

        public string GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        // User Timeline
        public string GetUserTimeline(IUser user, int maximumTweets = 40)
        {
            var userIdentifier = _userFactory.GetUserIdentifierFromUser(user);
            return GetUserTimeline(userIdentifier, maximumTweets);
        }

        public string GetUserTimeline(IUserIdentifier userIdentifier, int maximumTweets = 40)
        {
            var requestParameters = _userTimelineRequestParameterFactory.Create();
            requestParameters.UserIdentifier = userIdentifier;
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumTweets;

            return GetUserTimeline(requestParameters);
        }

        public string GetUserTimeline(long userId, int maximumTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserTimeline(userIdentifier, maximumTweets);
        }

        public string GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return GetUserTimeline(userIdentifier, maximumTweets);
        }

        public string GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters)
        {
            var query = _timelineQueryGenerator.GetUserTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        // Mentions Timeline
        public string GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var requestParameters = _mentionsTimelineRequestParameterFactory.Create();
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetMentionsTimeline(requestParameters);
        }

        public string GetMentionsTimeline(IMentionsTimelineRequestParameters timelineRequestParameters)
        {
            var query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }
    }
}