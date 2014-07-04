using System;
using System.ComponentModel;

namespace Chicken4WP8.Models
{
    public interface ITweetModel : INotifyPropertyChanged
    {
        long Id { get; }
        DateTime CreatedAt { get; }
        IUserModel User { get; }
        string Text { get; }
        bool IncludeMedia { get; }
        IEntities Entities { get; }
        bool IsRetweeted { get; }
        bool IsFavorited { get; }
        int RetweetCount { get; }
        int FavoriteCount { get; }
        Uri SourceUrl { get; }
        long InReplyToTweetId { get; }
        bool IncludeCoordinates { get; }
        ICoordinates Coordinates { get; }
        /// <summary>
        ///show retweet count, favorite count and location panel
        /// </summary>
        bool NeedShowRetweetIcons { get; }
    }
}
