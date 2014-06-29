using System;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Chicken4WP8.ViewModels.Base
{
    public class TweetModel : PropertyChangedBase
    {
        #region private
        protected ITweet tweet;
        protected UserModel user;
        #endregion

        public TweetModel(ITweet tweet)
        {
            this.tweet = tweet;
            this.user = new UserModel(this.tweet.Creator);
        }

        public ITweet Tweet
        {
            get { return tweet; }
        }

        public long Id
        {
            get { return tweet.Id; }
        }

        public DateTime CreatedAt
        {
            get { return tweet.CreatedAt; }
        }

        public UserModel Creator
        {
            get { return user; }
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
                    && this.tweet.Entities.Medias != null
                    && this.tweet.Entities.Medias.Count != 0;
            }
        }

        public ITweetEntities Entities
        {
            get { return this.tweet.Entities; }
        }

        public bool IsRetweeted
        {
            get { return tweet.Retweeted; }
        }

        public bool IsFavorited
        {
            get { return tweet.Favourited; }
        }

        public int RetweetCount
        {
            get { return tweet.RetweetCount; }
        }

        public int FavoriteCount
        {
            get { return tweet.FavouriteCount; }
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
            get { return tweet.Coordinates; }
        }

        /// <summary>
        ///show retweet count, favorite count and location panel
        /// </summary>
        public bool NeedShowRetweetIcons
        {
            get { return RetweetCount != 0 || FavoriteCount != 0 || IncludeCoordinates; }
        }
    }
}