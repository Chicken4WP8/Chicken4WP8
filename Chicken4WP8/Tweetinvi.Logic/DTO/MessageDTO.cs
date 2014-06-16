﻿using System;
using Newtonsoft.Json;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO
{
    public class MessageDTO : IMessageDTO
    {
        public bool IsMessagePublished { get; set; }
        public bool IsMessageDestroyed { get; set; }

        private long _id;

        [JsonProperty("id")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (_id != TweetinviConfig.DEFAULT_ID)
                {
                    IsMessagePublished = true;
                }
            }
        }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonTwitterDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("entities")]
        public ITweetEntities Entities { get; set; }

        [JsonProperty("sender_id")]
        public long SenderId { get; set; }

        [JsonProperty("sender_screen_name")]
        public string SenderScreenName { get; set; }

        [JsonProperty("sender")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUserDTO Sender { get; set; }

        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }

        [JsonProperty("recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        [JsonProperty("recipient")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUserDTO Recipient { get; set; }
    }
}