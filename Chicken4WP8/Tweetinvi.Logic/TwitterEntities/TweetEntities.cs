using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// Class storing multiple types of TweetEntities
    /// https://dev.twitter.com/docs/tweet-entities
    /// </summary>
    public class TweetEntities : ITweetEntities
    {
        [JsonProperty("urls")]
        public List<IUrlEntity> Urls { get; set; }

        [JsonProperty("media")]
        public List<IMediaEntity> Medias { get; set; }

        [JsonProperty("user_mentions")]
        public List<IUserMentionEntity> UserMentions { get; set; }

        [JsonProperty("hashtags")]
        public List<IHashtagEntity> Hashtags { get; set; }
    }
}