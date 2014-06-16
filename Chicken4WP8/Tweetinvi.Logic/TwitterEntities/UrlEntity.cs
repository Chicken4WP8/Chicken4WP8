using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// Object storing information related with an URL on twitter
    /// </summary>
    public class UrlEntity : IUrlEntity
    {
        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("display_url")]
        public string DisplayedURL { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedURL { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}