using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Logic.Model.Parameters
{
    public class UserTimelineRequestParameters : TimelineRequestParameters, IUserTimelineRequestParameters
    {
        public UserTimelineRequestParameters()
        {
            IncludeRTS = true;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool IncludeRTS { get; set; }
        public bool ExcludeReplies { get; set; }
    }
}