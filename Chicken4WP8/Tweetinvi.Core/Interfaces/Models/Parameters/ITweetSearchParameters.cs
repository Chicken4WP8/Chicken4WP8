using System;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Models.Parameters
{
    public enum TweetSearchFilter
    {
        All,
        OriginalTweetsOnly,
        RetweetsOnly,
    }

    public interface ITweetSearchParameters
    {
        string SearchQuery { get; set; }
    
        string Locale { get; set; }
        Language Lang { get; set; }
        IGeoCode GeoCode { get; set; }
        SearchResultType SearchType { get; set; }

        int MaximumNumberOfResults { get; set; }
        
        DateTime Since { get; set; }
        DateTime Until { get; set; }

        long SinceId { get; set; }
        long MaxId { get; set; }

        TweetSearchFilter TweetSearchFilter { get; set; }

        void SetGeoCode(ICoordinates coordinates, int radius, DistanceMeasure measure);
        void SetGeoCode(double longitude, double latitude, int radius, DistanceMeasure measure);

        void ClearGeoCode();
    }
}