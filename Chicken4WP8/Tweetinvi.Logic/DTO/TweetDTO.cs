﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO
{
    public class TweetDTO : TweetIdentifier, ITweetDTO
    {
        public bool IsTweetDestroyed { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("favorited")]
        public bool Favourited { get; set; }

        [JsonProperty("favorite_count")]
        public int FavouriteCount { get; set; }

        [JsonProperty("user")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUserDTO Creator { get; set; }

		[JsonProperty("current_user_retweet")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetIdentifier CurrentUserRetweetIdentifier { get; set; }

        [JsonProperty("coordinates")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ICoordinates Coordinates { get; set; }

        [JsonProperty("entities")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetEntities Entities { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonTwitterDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public long? InReplyToStatusId { get; set; }

        [JsonProperty("in_reply_to_status_id_str")]
        public string InReplyToStatusIdStr { get; set; }

        [JsonProperty("in_reply_to_user_id")]
        public long? InReplyToUserId { get; set; }

        [JsonProperty("in_reply_to_user_id_str")]
        public string InReplyToUserIdStr { get; set; }

        [JsonProperty("in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        [JsonProperty("retweet_count")]
        public int RetweetCount { get; set; }

        [JsonProperty("retweeted")]
        public bool Retweeted { get; set; }

        [JsonProperty("retweeted_status")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetDTO RetweetedTweetDTO { get; set; }

        [JsonProperty("possibly_sensitive")]
        public bool PossiblySensitive { get; set; }

        [JsonProperty("lang")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public Language Language { get; set; }

        [JsonProperty("contributorsIds")]
        public int[] ContributorsIds { get; set; }

        [JsonProperty("contributors")]
        public IEnumerable<long> Contributors { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("place")]
        public IPlace Place { get; set; }

        [JsonProperty("scopes")]
        public Dictionary<string, object> Scopes { get; set; }

        [JsonProperty("filter_level")]
        public string FilterLevel { get; set; }

        [JsonProperty("withheld_copyright")]
        public bool WithheldCopyright { get; set; }

        [JsonProperty("withheld_in_countries")]
        public IEnumerable<string> WithheldInCountries { get; set; }

        [JsonProperty("withheld_scope")]
        public string WithheldScope { get; set; }

        [JsonIgnore]
        public string PlaceId { get; set; }

        [JsonIgnore]
        public List<IMedia> MediasToPublish { get; set; }
    }
}