using System;
using Tweetinvi.Controllers.Properties;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryParameterGenerator
    {
        string GenerateExcludeRepliesParameter(bool excludeReplies);
        string GenerateIncludeContributorDetailsParameter(bool includeContributorDetails);
        string GenerateIncludeRTSParameter(bool includeRTS);
    }

    public class TimelineQueryParameterGenerator : ITimelineQueryParameterGenerator
    {
        public string GenerateExcludeRepliesParameter(bool excludeReplies)
        {
            return String.Format(Resources.TimelineParameter_ExcludeReplies, excludeReplies);
        }

        public string GenerateIncludeContributorDetailsParameter(bool includeContributorDetails)
        {
            return String.Format(Resources.TimelineParameter_IncludeContributorDetails, includeContributorDetails);
        }

        public string GenerateIncludeRTSParameter(bool includeRTS)
        {
            return String.Format(Resources.TimelineParameter_IncludeRTS, includeRTS);
        }
    }
}