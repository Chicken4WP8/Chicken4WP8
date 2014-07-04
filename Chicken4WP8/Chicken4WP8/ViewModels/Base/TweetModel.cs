﻿using System;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Models;
using CoreTweet;

namespace Chicken4WP8.ViewModels.Base
{
    public class TweetModel : PropertyChangedBase,ITweetModel
    {
        #region private
        private Status tweet;
        private IUserModel user;
        private IEntities entities;
        #endregion

        public TweetModel(Status tweet)
        {
            this.tweet = tweet;
            this.user = new UserModel(this.tweet.User);
        }

        public long Id
        {
            get { return tweet.Id; }
        }

        public DateTime CreatedAt
        {
            get { return tweet.CreatedAt.DateTime; }
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
            get { return this.tweet.Entities; }
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

        public Coordinates Coordinates
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