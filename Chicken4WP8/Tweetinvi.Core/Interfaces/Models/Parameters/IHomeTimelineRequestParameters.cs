namespace Tweetinvi.Core.Interfaces.Models.Parameters
{
    public interface IHomeTimelineRequestParameters : ITimelineRequestParameters
    {
        bool ExcludeReplies { get; set; }
    }
}