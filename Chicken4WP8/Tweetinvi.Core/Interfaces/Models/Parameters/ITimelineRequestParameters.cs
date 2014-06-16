namespace Tweetinvi.Core.Interfaces.Models.Parameters
{
    public interface ITimelineRequestParameters
    {
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        long SinceId { get; set; }
        long MaxId { get; set; }

        bool TrimUser { get; set; }
        bool IncludeContributorDetails { get; set; }
        bool IncludeEntities { get; set; }
    }
}