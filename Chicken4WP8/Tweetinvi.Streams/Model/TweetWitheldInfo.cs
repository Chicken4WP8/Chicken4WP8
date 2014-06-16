﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Streams.Model
{
    public class TweetWitheldInfo : ITweetWitheldInfo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("withheld_in_countries")]
        public IEnumerable<string> WitheldInCountries { get; set; }
    }
}