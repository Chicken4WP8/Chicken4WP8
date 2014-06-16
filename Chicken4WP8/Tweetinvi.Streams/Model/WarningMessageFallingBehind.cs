﻿using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Streams.Model
{
    public class WarningMessageFallingBehind : WarningMessage, IWarningMessageFallingBehind
    {
        [JsonProperty("percent_full")]
        public int PercentFull { get; set; }
    }
}