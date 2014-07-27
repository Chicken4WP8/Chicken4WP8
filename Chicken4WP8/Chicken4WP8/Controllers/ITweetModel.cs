using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface ITweetModel : INotifyPropertyChanged
    {
        long Id { get; set; }
        DateTime CreatedAt { get; set; }
        IUserModel User { get; set; }
        string Text { get; set; }
        IEntities Entities { get; set; }
        bool? IsRetweeted { get; set; }
        bool? IsFavorited { get; set; }
        int? RetweetCount { get; set; }
        ITweetModel RetweetedStatus { get; set; }
        int? FavoriteCount { get; set; }
        string Source { get; set; }
        Uri SourceUrl { get; set; }
        long? InReplyToTweetId { get; set; }
        string InReplyToScreenName { get; set; }
        ICoordinates Coordinates { get; set; }
        #region for template
        bool IncludeCoordinates { get; }
        bool IncludeMedia { get; }
        string InReplyToDisplayName { get; }
        List<IEntity> ParsedEntities { get; }
        /// <summary>
        ///show retweet count, favorite count and location panel
        /// </summary>
        bool NeedShowRetweetIcons { get; }
        /// <summary>
        /// show load more tweets button
        /// </summary>
        bool IsLoadMoreTweetButtonVisible { get; set; }
        /// <summary>
        /// show top bounds
        /// </summary>
        bool IsTopBoundsVisible { get; set; }
        /// <summary>
        /// show bottom bounds
        /// </summary>
        bool IsBottomBoundsVisible { get; set; }
        /// <summary>
        /// whether the current model is in status detail page or not.
        /// </summary>
        bool IsStatusDetail { get; set; }
        #endregion
    }
}
