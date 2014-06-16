﻿using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// ... Well a Tweet :)
    /// https://dev.twitter.com/docs/platform-objects/tweets
    /// </summary>
    public interface ITweet : ITweetIdentifier, ITweetAsync, IEquatable<ITweet>
    {
        #region Twitter API Properties

        /// <summary>
        /// Creation date of the Tweet
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Formatted text of the tweet
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Coordinates of the location from where the tweet has been sent
        /// </summary>
        ICoordinates Coordinates { get; set; }

        /// <summary>
        /// source field
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// Whether a tweet
        /// </summary>
        bool Truncated { get; }

        /// <summary>
        /// In_reply_to_status_id
        /// </summary>
        long? InReplyToStatusId { get; set; }

        /// <summary>
        /// In_reply_to_status_id_str
        /// </summary>
        string InReplyToStatusIdStr { get; set; }

        /// <summary>
        /// In_reply_to_user_id
        /// </summary>
        long? InReplyToUserId { get; set; }

        /// <summary>
        /// In_reply_to_user_id_str
        /// </summary>
        string InReplyToUserIdStr { get; set; }

        /// <summary>
        /// In_reply_to_screen_name
        /// </summary>
        string InReplyToScreenName { get; set; }

        /// <summary>
        /// User who created the Tweet
        /// </summary>
        IUser Creator { get; }

        /// <summary>
        /// Details the Tweet ID of the user's own retweet (if existent) of this Tweet.
        /// </summary>
        ITweetIdentifier CurrentUserRetweetIdentifier { get; }

        /// <summary>
        /// Ids of the users who contributed in the Tweet
        /// </summary>
        int[] ContributorsIds { get; }

        /// <summary>
        /// Users who contributed to the authorship of the tweet, on behalf of the official tweet author.
        /// </summary>
        IEnumerable<long> Contributors { get; }

        /// <summary>
        /// Number of retweets related with this tweet
        /// </summary>
        int RetweetCount { get; }

        /// <summary>
        /// Entities contained in the tweet
        /// </summary>
        ITweetEntities Entities { get; set; }

        /// <summary>
        /// Is the tweet favourited
        /// </summary>
        bool Favourited { get; }

        /// <summary>
        /// Number of time the tweet has been favourited
        /// </summary>
        int FavouriteCount { get; }

        /// <summary>
        /// Has the tweet been retweeted
        /// </summary>
        bool Retweeted { get; }

        /// <summary>
        /// Is the tweet potentialy sensitive
        /// </summary>
        bool PossiblySensitive { get; }

        /// <summary>
        /// Main language used in the tweet
        /// </summary>
        Language Language { get; }

        /// <summary>
        /// Geographic details concerning the location where the tweet has been published
        /// </summary>
        IPlace Place { get; }

        /// <summary>
        /// Informed whether a tweet is displayed or not in a specific type of scope. This property is most of the time null.
        /// </summary>
        Dictionary<string, object> Scopes { get; }

        /// <summary>
        /// Streaming tweets requires a filter level. A tweet will be streamed if its filter level is higher than the one of the stream
        /// </summary>
        string FilterLevel { get; }

        /// <summary>
        /// Informs that a tweet has been withheld for a copyright reason
        /// </summary>
        bool WithheldCopyright { get; }

        /// <summary>
        /// Countries in which the tweet will be withheld
        /// </summary>
        IEnumerable<string> WithheldInCountries { get; }

        /// <summary>
        /// When present, indicates whether the content being withheld is the "status" or a "user."
        /// </summary>
        string WithheldScope { get; }

        #endregion

        #region Tweetinvi API Properties

        ITweetDTO TweetDTO { get; set; }

        /// <summary>
        /// Determine the length of the Text using Twitter algorithm
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Date when the Tweet has been created in the program
        /// </summary>
        DateTime TweetLocalCreationDate { get; }

        /// <summary>
        /// Collection of hashtags associated with a Tweet
        /// </summary>
        List<IHashtagEntity> Hashtags { get; set; }

        /// <summary>
        /// Collection of urls associated with a tweet
        /// </summary>
        List<IUrlEntity> Urls { get; set; }

        /// <summary>
        /// Collection of medias associated with a tweet
        /// </summary>
        List<IMediaEntity> Media { get; set; }

        /// <summary>
        /// Collection of tweets mentioning this tweet
        /// </summary>
        List<IUserMentionEntity> UserMentions { get; set; }

        /// <summary>
        /// Collection of tweets retweeting this tweet
        /// </summary>
        List<ITweet> Retweets { get; set; }

        bool IsRetweet { get; }

        /// <summary>
        /// If the tweet is a retweet this field provides
        /// the tweet that it retweeted
        /// </summary>
        ITweet RetweetedTweet { get; }

        /// <summary>
        /// Inform us if this tweet has been published on Twitter
        /// </summary>
        bool IsTweetPublished { get; }

        /// <summary>
        /// Inform us if this tweet was destroyed
        /// </summary>
        bool IsTweetDestroyed { get; }

        #endregion

        #region Publish

        /// <summary>
        /// Returns the number of characters remaining for the Tweet to be authorized by Twitter
        /// </summary>
        int TweetRemainingCharacters();

        /// <summary>
        /// Add a media to be published with the tweet.
        /// Authorized extensions : .PNG, .JPG, .GIF (non-animated)
        /// </summary>
        void AddMedia(byte[] data);

        /// <summary>
        /// Publish to the twitter Timeline of the logged user
        /// </summary>
        bool Publish();

        /// <summary>
        /// Publish a reply to the current tweet
        /// </summary>
        ITweet PublishReply(string text);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishReply(ITweet replyTweet);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishInReplyTo(long replyToTweetId);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishInReplyTo(ITweet replyToTweet);

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        bool PublishWithGeo(ICoordinates coordinates);

        /// <summary>
        /// Publish a Tweet created from the API
        /// </summary>
        bool PublishWithGeo(double longitude, double latitude);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishWithGeoInReplyTo(ICoordinates coordinates, ITweet replyToTweet);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishWithGeoInReplyTo(double longitude, double latitude, long replyToTweetId);

        /// <summary>
        /// Publish this tweet in reply to another tweet
        /// </summary>
        bool PublishWithGeoInReplyTo(double longitude, double latitude, ITweet replyToTweet);

        #endregion

        #region Favourites

        /// <summary>
        /// Favorites the tweet
        /// </summary>
        void Favourite();

        /// <summary>
        /// Remove the tweet from favourites
        /// </summary>
        void UnFavourite();

        #endregion

        /// <summary>
        /// Retweet the current tweet from the currently logged user
        /// </summary>
        ITweet PublishRetweet();

        /// <summary>
        /// Get the retweets of the current tweet
        /// </summary>
        List<ITweet> GetRetweets();

        /// <summary>
        /// Delete a tweet from Twitter
        /// </summary>
        bool Destroy();

        /// <summary>
        /// Generate an OEmbedTweet
        /// </summary>
        IOEmbedTweet GenerateOEmbedTweet();
    }
}