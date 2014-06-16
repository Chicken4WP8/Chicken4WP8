using System;
using Newtonsoft.Json;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO
{
    public class TweetListDTO : ITweetListDTO
    {
        public TweetListDTO()
        {
            Id = TweetinviConfig.DEFAULT_ID;
        }

        [JsonProperty("id")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("user")]
        public IUserDTO Creator { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonTwitterDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("mode")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public PrivacyMode PrivacyMode { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("subscriber_count")]
        public int SubscriberCount { get; set; }
    }
}