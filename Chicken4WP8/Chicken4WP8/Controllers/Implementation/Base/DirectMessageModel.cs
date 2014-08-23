using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CoreTweet;
using Newtonsoft.Json;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class DirectMessageModel : PropertyChangedBase, IDirectMessageModel
    {
        public DirectMessageModel()
        { }

        public DirectMessageModel(DirectMessage message)
        {
            CreatedAt = message.CreatedAt.DateTime.ToLocalTime();
            Id = message.Id;
            Text = message.Text;
            if (message.Entities != null)
                Entities = new EntitiesModel(message.Entities);
            if (message.Sender != null && message.Sender.Id == App.UserSetting.Id)
                IsSentByMe = true;
        }

        public DateTime CreatedAt { get; set; }
        public IEntities Entities { get; set; }
        public long Id { get; set; }
        public IUserModel User { get; set; }
        public string Text { get; set; }
        public bool IsSentByMe { get; set; }

        private List<IEntity> parsedEntities;
        [JsonIgnore]
        public List<IEntity> ParsedEntities
        {
            get
            {
                if (Entities == null) return null;
                if (parsedEntities != null) return parsedEntities;

                parsedEntities = new List<IEntity>();
                if (Entities.HashTags != null && Entities.HashTags.Count != 0)
                    parsedEntities.AddRange(Utils.ParseHashTags(Text, Entities.HashTags));
                if (Entities.Media != null && Entities.Media.Count != 0)
                    parsedEntities.AddRange(Utils.ParseMedias(Text, Entities.Media));
                if (Entities.Symbols != null && Entities.Symbols.Count != 0)
                    parsedEntities.AddRange(Utils.ParseSymbols(Text, Entities.Symbols));
                if (Entities.Urls != null && Entities.Urls.Count != 0)
                    parsedEntities.AddRange(Utils.ParseUrls(Text, Entities.Urls));
                if (Entities.UserMentions != null && Entities.UserMentions.Count != 0)
                    parsedEntities.AddRange(Utils.ParseUserMentions(Text, Entities.UserMentions));
                return parsedEntities;
            }
        }
    }
}
