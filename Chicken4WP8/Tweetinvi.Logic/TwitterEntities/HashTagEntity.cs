using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// A hashtag is a keyword prefixed by # and representing a category of tweet
    /// This class stores information related with an user mention
    /// </summary>
    public class HashtagEntity : IHashtagEntity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}