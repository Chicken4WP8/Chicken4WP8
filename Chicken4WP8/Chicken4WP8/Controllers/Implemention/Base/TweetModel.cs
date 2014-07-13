using System;
using Caliburn.Micro;
using Chicken4WP8.Common;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class TweetModel : PropertyChangedBase, ITweetModel
    {
        #region private
        private Status tweet;
        private IUserModel user;
        private IEntities entities;
        private ICoordinates coordinates;
        #endregion

        public TweetModel(Status tweet)
        {
            this.tweet = tweet;
            this.user = new UserModel(tweet.User);
            if (tweet.Entities != null)
                this.entities = new EntitiesModel(tweet.Entities);
            if (tweet.Coordinates != null)
                this.coordinates = new CoordinatesModel(tweet.Coordinates);
        }

        public long? Id
        {
            get { return tweet.Id; }
        }

        public DateTime CreatedAt
        {
            get { return tweet.CreatedAt.LocalDateTime; }
        }

        public IUserModel User
        {
            get { return this.user; }
        }

        public string Text
        {
            get { return tweet.Text; }
        }

        public bool IncludeMedia
        {
            get
            {
                return this.tweet.Entities != null
                    && this.tweet.Entities.Media != null
                    && this.tweet.Entities.Media.Length != 0;
            }
        }

        public IEntities Entities
        {
            get { return entities; }
        }

        public bool? IsRetweeted
        {
            get { return tweet.IsRetweeted; }
        }

        public bool? IsFavorited
        {
            get { return tweet.IsFavorited; }
        }

        public int? RetweetCount
        {
            get { return tweet.RetweetCount; }
        }

        public int? FavoriteCount
        {
            get { return tweet.FavoriteCount; }
        }

        public string Source
        {
            get { return TwitterHelper.ParseToSource(tweet.Source); }
        }

        public Uri SourceUrl
        {
            get { return new Uri(TwitterHelper.ParseToSourceUrl(tweet.Source), UriKind.Absolute); }
        }

        public long? InReplyToTweetId
        {
            get { return tweet.InReplyToStatusId; }
        }

        public bool IncludeCoordinates
        {
            get { return tweet.Coordinates != null; }
        }

        public ICoordinates Coordinates
        {
            get { return coordinates; }
        }

        /// <summary>
        ///show retweet count, favorite count and location panel
        /// </summary>
        public bool NeedShowRetweetIcons
        {
            get { return RetweetCount != 0 || FavoriteCount != 0 || IncludeCoordinates; }
        }

        private bool isLoadMoreTweetButtonVisible;
        public bool IsLoadMoreTweetButtonVisible
        {
            get { return isLoadMoreTweetButtonVisible; }
            set
            {
                isLoadMoreTweetButtonVisible = value;
                NotifyOfPropertyChange(() => IsLoadMoreTweetButtonVisible);
            }
        }

        private bool isBottomBoundsVisible;
        public bool IsBottomBoundsVisible
        {
            get { return isBottomBoundsVisible; }
            set
            {
                isBottomBoundsVisible = value;
                NotifyOfPropertyChange(() => IsBottomBoundsVisible);
            }
        }

        private bool isTopBoundsVisible;
        public bool IsTopBoundsVisible
        {
            get { return isTopBoundsVisible; }
            set
            {

                isTopBoundsVisible = value;
                NotifyOfPropertyChange(() => IsTopBoundsVisible);
            }
        }
    }
}