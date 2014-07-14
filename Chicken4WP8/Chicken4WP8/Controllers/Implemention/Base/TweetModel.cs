using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            get { return Const.ParseToSource(tweet.Source); }
        }

        public Uri SourceUrl
        {
            get { return new Uri(Const.ParseToSourceUrl(tweet.Source), UriKind.Absolute); }
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
                    parsedEntities.AddRange(ParseHashTags(Text, Entities.HashTags));
                if (Entities.Media != null && Entities.Media.Count != 0)
                    parsedEntities.AddRange(ParseMedias(Text, Entities.Media));
                if (Entities.Symbols != null && Entities.Symbols.Count != 0)
                    parsedEntities.AddRange(ParseSymbols(Text, Entities.Symbols));
                if (Entities.Urls != null && Entities.Urls.Count != 0)
                    parsedEntities.AddRange(ParseUrls(Text, Entities.Urls));
                if (Entities.UserMentions != null && Entities.UserMentions.Count != 0)
                    parsedEntities.AddRange(ParseUserMentions(Text, Entities.UserMentions));
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

        #region parse entities
        private static IEnumerable<IEntity> ParseUserMentions(string text, IList<IUserMentionEntity> mentions)
        {
            foreach (var mention in mentions.Distinct())
            {
                var matches = Regex.Matches(text, string.Format(Const.USERNAMEPATTERN, Regex.Escape(mention.DisplayText)), RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var entity = new UserMentionEntity
                    {
                        Id = mention.Id,
                        Name = mention.Name,
                        ScreenName = mention.ScreenName,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new UserMentionEntityModel(entity);
                    yield return model;
                }
            }
        }

        private static IEnumerable<IEntity> ParseHashTags(string text, IList<ISymbolEntity> hashtags)
        {
            foreach (var hashtag in hashtags.Distinct())
            {
                var matches = Regex.Matches(text, string.Format(Const.HASHTAGPATTERN, Regex.Escape(hashtag.DisplayText)));
                foreach (Match match in matches)
                {
                    var entity = new SymbolEntity
                    {
                        Text = hashtag.Text,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new HashTagEntityModel(entity);
                    yield return model;
                }
            }
        }

        private static IEnumerable<IEntity> ParseSymbols(string text, IList<ISymbolEntity> symbols)
        {
            foreach (var symbol in symbols.Distinct())
            {
                var matches = Regex.Matches(text, string.Format(Const.HASHTAGPATTERN, Regex.Escape(symbol.DisplayText)));
                foreach (Match match in matches)
                {
                    var entity = new SymbolEntity
                    {
                        Text = symbol.Text,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new SymbolEntityModel(entity);
                    yield return model;
                }
            }
        }

        private static IEnumerable<IEntity> ParseUrls(string text, IList<IUrlEntity> urls)
        {
            foreach (var url in urls.Distinct())
            {
                var matches = Regex.Matches(text, string.Format(Const.URLPATTERN, Regex.Escape(url.Url.AbsoluteUri)));
                foreach (Match match in matches)
                {
                    var entity = new UrlEntity
                    {
                        DisplayUrl = url.DisplayUrl,
                        ExpandedUrl = url.ExpandedUrl,
                        Url = url.Url,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new UrlEntityModel(entity);
                    yield return model;
                }
            }
        }

        private static IEnumerable<IEntity> ParseMedias(string text, IList<IMediaEntity> medias)
        {
            foreach (var media in medias)
            {
                var matches = Regex.Matches(text, string.Format(Const.URLPATTERN, Regex.Escape(media.Url.AbsoluteUri)));
                foreach (Match match in matches)
                {
                    var entity = new MediaEntity
                    {
                        Id = media.Id,
                        MediaUrl = media.MediaUrl,
                        MediaUrlHttps = media.MediaUrlHttps,
                        SourceStatusId = media.SourceStatusId,
                        Type = media.Type,
                        DisplayUrl = media.DisplayUrl,
                        ExpandedUrl = media.ExpandedUrl,
                        Url = media.Url,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new MediaEntityModel(entity) { Sizes = media.Sizes };
                    yield return model;
                }
            }
        }
        #endregion
    }
}