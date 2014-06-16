using System.Collections.Generic;
using Newtonsoft.Json;
using TweetinCore.Interfaces.DTO;
using Tweetinvi.DTO;
using Tweetinvi.JsonConverters;

namespace TweetinviFactories.DTO
{
    public class TweetEntitiesDTO : ITweetEntitiesDTO
    {
        [JsonProperty("urls")]
        [JsonConverter(typeof(JsonTwitterConverterRepository))]
        public List<IUrlEntityDTO> Urls { get; set; }

        [JsonProperty("hashtags")]
        [JsonConverter(typeof(JsonTwitterConverterRepository))]
        public List<IHashtagEntityDTO> Hashtags { get; set; }
    }
}
