using System;
using System.Collections.Generic;
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
        private ITweetModel retweetedStatus;
        private IEntities entities;
        private ICoordinates coordinates;
        #endregion

        public TweetModel(Status tweet)
        {
            this.tweet = tweet;
            this.user = new UserModel(tweet.User);
            if (tweet.RetweetedStatus != null)
                this.retweetedStatus = new TweetModel(tweet.RetweetedStatus);
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

        public ITweetModel RetweetedStatus
        {
            get { return retweetedStatus; }
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

        private List<IEntity> parsedEntities;
        public List<IEntity> ParsedEntities
        {
            get
            {
                if (Entities == null) return null;
                if (parsedEntities != null) return parsedEntities;

                parsedEntities = new List<IEntity>();
                if (Entities.HashTags != null && Entities.HashTags.Count != 0)
                    parsedEntities.AddRange(Entities.HashTags);
                if (Entities.Media != null && Entities.Media.Count != 0)
                    parsedEntities.AddRange(Entities.Media);
                if (Entities.Symbols != null && Entities.Symbols.Count != 0)
                    parsedEntities.AddRange(Entities.Symbols);
                if (Entities.Urls != null && Entities.Urls.Count != 0)
                    parsedEntities.AddRange(Entities.Urls);
                if (Entities.UserMentions != null && Entities.UserMentions.Count != 0)
                    parsedEntities.AddRange(Entities.UserMentions);
                return parsedEntities;
            }
        }

        #region for template
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
        #endregion
    }
}