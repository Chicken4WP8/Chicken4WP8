using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Logic.Model.Parameters
{
    public class HomeTimelineRequestParameters : TimelineRequestParameters, IHomeTimelineRequestParameters
    {
        public bool ExcludeReplies { get; set; }
    }
}