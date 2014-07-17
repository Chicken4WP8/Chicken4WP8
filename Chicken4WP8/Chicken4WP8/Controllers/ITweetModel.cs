using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface ITweetModel : INotifyPropertyChanged
    {
        long? Id { get; }
        DateTime CreatedAt { get; }
        IUserModel User { get; }
        string Text { get; }
        bool IncludeMedia { get; }
        IEntities Entities { get; }
        bool? IsRetweeted { get; }
        bool? IsFavorited { get; }
        int? RetweetCount { get; }
        ITweetModel RetweetedStatus { get; }
        int? FavoriteCount { get; }
        string Source { get; }
        Uri SourceUrl { get; }
        long? InReplyToTweetId { get; }
        bool IncludeCoordinates { get; }
        ICoordinates Coordinates { get; }

        List<IEntity> ParsedEntities { get; }

        #region for template
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
