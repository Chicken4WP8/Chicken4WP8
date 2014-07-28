using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class EntitiesModel : PropertyChangedBase, IEntities
    {
        public EntitiesModel()
        { }

        public EntitiesModel(CoreTweet.Entities entities)
        {
            if (entities != null)
            {
                if (entities.HashTags != null && entities.HashTags.Length != 0)
                {
                    HashTags = new List<ISymbolEntity>();
                    foreach (var hashTag in entities.HashTags)
                        HashTags.Add(new HashTagEntityModel(hashTag));
                }
                if (entities.Media != null && entities.Media.Length != 0)
                {
                    Media = new List<IMediaEntity>();
                    foreach (var m in entities.Media)
                        Media.Add(new MediaEntityModel(m));
                }
                if (entities.Symbols != null && entities.Symbols.Length != 0)
                {
                    Symbols = new List<ISymbolEntity>();
                    foreach (var symbol in entities.Symbols)
                        Symbols.Add(new SymbolEntityModel(symbol));
                }
                if (entities.Urls != null && entities.Urls.Length != 0)
                {
                    Urls = new List<IUrlEntity>();
                    foreach (var url in entities.Urls)
                        Urls.Add(new UrlEntityModel(url));
                }
                if (entities.UserMentions != null && entities.UserMentions.Length != 0)
                {
                    UserMentions = new List<IUserMentionEntity>();
                    foreach (var userMention in entities.UserMentions)
                        UserMentions.Add(new UserMentionEntityModel(userMention));
                }
            }
        }

        public List<ISymbolEntity> HashTags { get; set; }
        public List<IMediaEntity> Media { get; set; }
        public List<ISymbolEntity> Symbols { get; set; }
        public List<IUrlEntity> Urls { get; set; }
        public List<IUserMentionEntity> UserMentions { get; set; }
    }

    public abstract class EntityModel : PropertyChangedBase, IEntity
    {
        public EntityModel()
        { }

        public EntityModel(Entity entity)
        {
            Index = entity.Indices[0];
        }

        public int Index { get; set; }
        public abstract EntityType EntityType { get; }
        public abstract string DisplayText { get; }
    }

    public class SymbolEntityModel : EntityModel, ISymbolEntity
    {
        public SymbolEntityModel()
        { }

        public SymbolEntityModel(SymbolEntity entity)
            : base(entity)
        {
            Text = entity.Text;
        }

        public override EntityType EntityType
        {
            get { return EntityType.Symbol; }
        }
        public string Text { get; set; }
        public override string DisplayText
        {
            get { return "$" + Text; }
        }
    }

    public class HashTagEntityModel : SymbolEntityModel, ISymbolEntity
    {
        public HashTagEntityModel()
        { }

        public HashTagEntityModel(SymbolEntity entity)
            : base(entity)
        { }

        public override EntityType EntityType
        {
            get { return EntityType.HashTag; }
        }
        public override string DisplayText
        {
            get { return "#" + Text; }
        }
    }

    public class UrlEntityModel : EntityModel, IUrlEntity
    {
        public UrlEntityModel()
        { }

        public UrlEntityModel(UrlEntity entity)
            : base(entity)
        {
            DisplayUrl = entity.DisplayUrl;
            int index = DisplayUrl.IndexOf("/");
            if (index != -1)
                TruncatedUrl = "[" + DisplayUrl.Remove(index) + "]";
            else
                TruncatedUrl = "[" + DisplayUrl + "]";
            ExpandedUrl = entity.ExpandedUrl;
            Url = entity.Url;
        }

        public override EntityType EntityType
        {
            get { return EntityType.Url; }
        }
        public string DisplayUrl { get; set; }
        public string TruncatedUrl { get; set; }
        public Uri ExpandedUrl { get; set; }
        public Uri Url { get; set; }
        public override string DisplayText
        {
            get
            {
                if (Url != null)
                    return Url.AbsoluteUri;
                return string.Empty;
            }
        }
    }

    public class MediaEntityModel : UrlEntityModel, IMediaEntity
    {
        public MediaEntityModel()
        { }

        public MediaEntityModel(MediaEntity entity)
            : base(entity)
        {
            Id = entity.Id;
            MediaUrl = entity.MediaUrl;
            MediaUrlHttps = entity.MediaUrlHttps;
            SourceStatusId = entity.SourceStatusId;
            Type = entity.Type;
        }

        public override EntityType EntityType
        {
            get { return EntityType.Media; }
        }
        public long Id { get; set; }
        public Uri MediaUrl { get; set; }
        public Uri MediaUrlHttps { get; set; }
        public long? SourceStatusId { get; set; }
        public string Type { get; set; }
    }

    public class UserMentionEntityModel : EntityModel, IUserMentionEntity
    {
        public UserMentionEntityModel()
        { }

        public UserMentionEntityModel(UserMentionEntity entity)
            : base(entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ScreenName = entity.ScreenName;
        }

        public override EntityType EntityType
        {
            get { return EntityType.UserMention; }
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public override string DisplayText
        {
            get { return "@" + ScreenName; }
        }
    }

    public class CoordinatesModel : ICoordinates
    {
        public CoordinatesModel()
        { }

        public CoordinatesModel(Coordinates coordinates)
        {
            Latitude = coordinates.Latitude;
            Longtitude = coordinates.Longtitude;
            Type = coordinates.Type;
        }

        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return Latitude + "," + Longtitude;
        }
    }
}
