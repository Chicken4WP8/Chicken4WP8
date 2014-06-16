using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryExecutor
    {
        // Home Timeline
        IEnumerable<ITweetDTO> GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters);

        // User Timeline
        IEnumerable<ITweetDTO> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters);

        // Mention Timeline
        IEnumerable<ITweetDTO> GetMentionsTimeline(IMentionsTimelineRequestParameters timelineRequestParameters);
    }

    public class TimelineQueryExecutor : ITimelineQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ITimelineQueryGenerator _timelineQueryGenerator;

        public TimelineQueryExecutor(
            ITwitterAccessor twitterAccessor,
            ITimelineQueryGenerator timelineQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _timelineQueryGenerator = timelineQueryGenerator;
        }

        // Home Timeline
        public IEnumerable<ITweetDTO> GetHomeTimeline(IHomeTimelineRequestParameters timelineRequestParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // User Timeline
        public IEnumerable<ITweetDTO> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters)
        {
            string query = _timelineQueryGenerator.GetUserTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Mention Timeline
        public IEnumerable<ITweetDTO> GetMentionsTimeline(IMentionsTimelineRequestParameters timelineRequestParameters)
        {
            string query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineRequestParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }
    }
}