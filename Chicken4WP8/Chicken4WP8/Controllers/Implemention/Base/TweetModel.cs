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

        public TweetModel()
        {
            this.tweet = new Status();
        }

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

        public long Id
        {
            get { return tweet.Id; }
            set { tweet.Id = value; }
        }

        public DateTime CreatedAt
        {
            get { return tweet.CreatedAt.LocalDateTime; }
            set { tweet.CreatedAt = new DateTimeOffset(value); }
        }

        public IUserModel User
        {
            get { return this.user; }
            set { this.user = value; }
        }

        public string Text
        {
            get { return tweet.Text; }
            set { tweet.Text = value; }
        }

        public IEntities Entities
        {
            get { return entities; }
            set { entities = value; }
        }

        public bool? IsRetweeted
        {
            get { return tweet.IsRetweeted; }
            set { tweet.IsRetweeted = value; }
        }

        public bool? IsFavorited
        {
            get { return tweet.IsFavorited; }
            set { tweet.IsFavorited = value; }
        }

        public int? RetweetCount
        {
            get { return tweet.RetweetCount; }
            set { tweet.RetweetCount = value; }
        }

        public ITweetModel RetweetedStatus
        {
            get { return retweetedStatus; }
            set { retweetedStatus = value; }
        }

        public int? FavoriteCount
        {
            get { return tweet.FavoriteCount; }
            set { tweet.FavoriteCount = value; }
        }

        private string source;
        public string Source
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    source = Const.ParseToSource(tweet.Source);
                return source;
            }
            set { source = value; }
        }

        private Uri sourceUrl;
        public Uri SourceUrl
        {
            get
            {
                if (sourceUrl == null)
                    sourceUrl = new Uri(Const.ParseToSourceUrl(tweet.Source), UriKind.Absolute);
                return sourceUrl;
            }
            set { sourceUrl = value; }
        }

        public long? InReplyToTweetId
        {
            get { return tweet.InReplyToStatusId; }
            set { tweet.InReplyToStatusId = value; }
        }

        public ICoordinates Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        #region for template
        public bool IncludeMedia
        {
            get
            {
                return this.tweet.Entities != null
                    && this.tweet.Entities.Media != null
                    && this.tweet.Entities.Media.Length != 0;
            }
        }

        public bool IncludeCoordinates
        {
            get { return coordinates != null; }
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
                    parsedEntities.AddRange(Utils.ParseHashTags(Text, Entities.HashTags));
                if (Entities.Media != null && Entities.Media.Count != 0)
                    parsedEntities.AddRange(Utils.ParseMedias(Text, Entities.Media));
                if (Entities.Symbols != null && Entities.Symbols.Count != 0)
                    parsedEntities.AddRange(Utils.ParseSymbols(Text, Entities.Symbols));
                if (Entities.Urls != null && Entities.Urls.Count != 0)
                    parsedEntities.AddRange(Utils.ParseUrls(Text, Entities.Urls));
                if (Entities.UserMentions != null && Entities.UserMentions.Count != 0)
                    parsedEntities.AddRange(Utils.ParseUserMentions(Text, Entities.UserMentions));
                return parsedEntities;
            }
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

        private bool isStatusDetail;
        public bool IsStatusDetail
        {
            get { return isStatusDetail; }
            set
            {
                isStatusDetail = value;
                NotifyOfPropertyChange(() => IsStatusDetail);
            }
        }
        #endregion
    }
}