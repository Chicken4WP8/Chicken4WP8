using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Chicken4WP8.Common;
using CoreTweet;
using Newtonsoft.Json;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class TweetModel : PropertyChangedBase, ITweetModel
    {
        public TweetModel()
        { }

        public TweetModel(Status tweet)
        {
            Id = tweet.Id;
            CreatedAt = tweet.CreatedAt.LocalDateTime;
            User = new UserModel(tweet.User);
            Text = tweet.Text;
            Entities = new EntitiesModel(tweet.Entities);
            IsRetweeted = tweet.IsRetweeted;
            IsFavorited = tweet.IsFavorited;
            RetweetCount = tweet.RetweetCount;
            if (tweet.RetweetedStatus != null)
                RetweetedStatus = new TweetModel(tweet.RetweetedStatus);
            FavoriteCount = tweet.FavoriteCount;
            if (!string.IsNullOrEmpty(tweet.Source))
            {
                Source = Const.ParseToSource(tweet.Source);
                SourceUrl = new Uri(Const.ParseToSourceUrl(tweet.Source), UriKind.Absolute);
            }
            InReplyToTweetId = tweet.InReplyToStatusId;
            InReplyToScreenName = tweet.InReplyToScreenName;
            if (tweet.Coordinates != null)
                Coordinates = new CoordinatesModel(tweet.Coordinates);
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public IUserModel User { get; set; }
        public string Text { get; set; }
        public IEntities Entities { get; set; }
        public bool? IsRetweeted { get; set; }
        public bool? IsFavorited { get; set; }
        public int? RetweetCount { get; set; }
        public ITweetModel RetweetedStatus { get; set; }
        public int? FavoriteCount { get; set; }
        public string Source { get; set; }
        public Uri SourceUrl { get; set; }
        public long? InReplyToTweetId { get; set; }
        public string InReplyToScreenName { get; set; }
        public ICoordinates Coordinates { get; set; }
        #region for template
        [JsonIgnore]
        public bool IncludeMedia
        {
            get
            {
                return Entities != null
                    && Entities.Media != null
                    && Entities.Media.Count != 0;
            }
        }
        [JsonIgnore]
        public bool IncludeCoordinates
        {
            get { return Coordinates != null; }
        }
        [JsonIgnore]
        public string InReplyToDisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(InReplyToScreenName))
                    return "@" + InReplyToScreenName;
                return string.Empty;
            }
        }
        private List<IEntity> parsedEntities;
        [JsonIgnore]
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
        [JsonIgnore]
        public bool NeedShowRetweetIcons
        {
            get { return RetweetCount != 0 || FavoriteCount != 0 || IncludeCoordinates; }
        }
        private bool isLoadMoreTweetButtonVisible;
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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