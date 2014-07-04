using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Chicken4WP8.Models;
using CoreTweet;

namespace Chicken4WP8.ViewModels.Base
{
    public class EntitiesModel : PropertyChangedBase, IEntities
    {
        private CoreTweet.Entities entities;
        private IList<ISymbolEntity> hashTags;
        private IList<IMediaEntity> media;
        private IList<ISymbolEntity> symbols;
        private IList<IUrlEntity> urls;
        private IList<IUserMentionEntity> userMentions;

        public EntitiesModel(CoreTweet.Entities entities)
        {
            if (entities != null)
            {
                this.entities = entities;
                if (entities.HashTags != null && entities.HashTags.Length != 0)
                {
                    hashTags = new List<ISymbolEntity>();
                    foreach (var hashTag in entities.HashTags)
                        hashTags.Add(new SymbolEntityModel(hashTag));
                }
                if (entities.Media != null && entities.Media.Length != 0)
                {
                    media = new List<IMediaEntity>();
                    foreach (var m in entities.Media)
                        media.Add(new MediaEntityModel(m));
                }
                if (entities.Symbols != null && entities.Symbols.Length != 0)
                {
                    symbols = new List<ISymbolEntity>();
                    foreach (var symbol in entities.Symbols)
                        symbols.Add(new SymbolEntityModel(symbol));
                }
                if (entities.Urls != null && entities.Urls.Length != 0)
                {
                    urls = new List<IUrlEntity>();
                    foreach (var url in entities.Urls)
                        urls.Add(new UrlEntityModel(url));
                }
                if (entities.UserMentions != null && entities.UserMentions.Length != 0)
                {
                    userMentions = new List<IUserMentionEntity>();
                    foreach (var userMention in entities.UserMentions)
                        userMentions.Add(new UserMentionEntityModel(userMention));
                }
            }
        }

        public IList<ISymbolEntity> HashTags
        {
            get { return hashTags; }
            set { hashTags = value; }
        }

        public IList<IMediaEntity> Media
        {
            get { return media; }
            set { media = value; }
        }

        public IList<ISymbolEntity> Symbols
        {
            get { return symbols; }
            set { symbols = value; }
        }

        public IList<IUrlEntity> Urls
        {
            get { return urls; }
            set { urls = value; }
        }

        public IList<IUserMentionEntity> UserMentions
        {
            get { return userMentions; }
            set { userMentions = value; }
        }
    }

    public abstract class EntityModel : PropertyChangedBase, IEntity
    {
        private Entity entity;

        public EntityModel(Entity entity)
        {
            this.entity = entity;
        }

        public int[] Indices
        {
            get { return entity.Indices; }
            set { entity.Indices = value; }
        }
    }

    public class SymbolEntityModel : EntityModel, ISymbolEntity
    {
        private SymbolEntity entity;

        public SymbolEntityModel(SymbolEntity entity)
            : base(entity)
        {
            this.entity = entity;
        }

        public string Text
        {
            get { return entity.Text; }
            set { entity.Text = value; }
        }
    }

    #region Media Entity
    public class MediaEntityModel : EntityModel, IMediaEntity
    {
        private MediaEntity entity;
        private IMediaSizes sizes;

        public MediaEntityModel(MediaEntity entity)
            : base(entity)
        {
            this.entity = entity;
        }

        public long Id
        {
            get { return entity.Id; }
            set { entity.Id = value; }
        }

        public Uri MediaUrl
        {
            get { return entity.MediaUrl; }
            set { entity.MediaUrl = value; }
        }

        public Uri MediaUrlHttps
        {
            get { return entity.MediaUrlHttps; }
            set { entity.MediaUrlHttps = value; }
        }

        public IMediaSizes Sizes
        {
            get { return sizes; }
            set { sizes = value; }
        }

        public long? SourceStatusId
        {
            get { return entity.SourceStatusId; }
            set { entity.SourceStatusId = value; }
        }

        public string Type
        {
            get { return entity.Type; }
            set { entity.Type = value; }
        }
    }

    public class MediaSizesModel : PropertyChangedBase, IMediaSizes
    {
        private MediaSizes sizes;
        private IMediaSize large;
        private IMediaSize medium;
        private IMediaSize small;
        private IMediaSize thumb;

        public MediaSizesModel(MediaSizes sizes)
        {
            this.sizes = sizes;
            this.large = new MediaSizeModel(sizes.Large);
            this.medium = new MediaSizeModel(sizes.Medium);
            this.small = new MediaSizeModel(sizes.Small);
            this.thumb = new MediaSizeModel(sizes.Thumb);
        }

        public IMediaSize Large
        {
            get { return large; }
            set { large = value; }
        }

        public IMediaSize Medium
        {
            get { return medium; }
            set { medium = value; }
        }

        public IMediaSize Small
        {
            get { return small; }
            set { small = value; }
        }

        public IMediaSize Thumb
        {
            get { return thumb; }
            set { thumb = value; }
        }
    }

    public class MediaSizeModel : PropertyChangedBase, IMediaSize
    {
        private MediaSize size;

        public MediaSizeModel(MediaSize size)
        {
            this.size = size;
        }

        public int Height
        {
            get { return size.Height; }
            set { size.Height = value; }
        }

        public string Resize
        {
            get { return size.Resize; }
            set { size.Resize = value; }
        }

        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }
    }
    #endregion

    public class UrlEntityModel : EntityModel, IUrlEntity
    {
        private UrlEntity entity;

        public UrlEntityModel(UrlEntity entity)
            : base(entity)
        {
            this.entity = entity;
        }

        public string DisplayUrl
        {
            get { return entity.DisplayUrl; }
            set { entity.DisplayUrl = value; }
        }

        public Uri ExpandedUrl
        {
            get { return entity.ExpandedUrl; }
            set { entity.ExpandedUrl = value; }
        }

        public Uri Url
        {
            get { return entity.Url; }
            set { entity.Url = value; }
        }
    }

    public class UserMentionEntityModel : EntityModel, IUserMentionEntity
    {
        private UserMentionEntity entity;

        public UserMentionEntityModel(UserMentionEntity entity)
            : base(entity)
        {
            this.entity = entity;
        }

        public long Id
        {
            get { return entity.Id; }
            set { entity.Id = value; }
        }

        public string Name
        {
            get { return entity.Name; }
            set { entity.Name = value; }
        }

        public string ScreenName
        {
            get { return entity.ScreenName; }
            set { entity.ScreenName = value; }
        }
    }

    public class CoordinatesModel : ICoordinates
    {
        private Coordinates coordinates;

        public CoordinatesModel(Coordinates coordinates)
        {
            this.coordinates = coordinates;
        }

        public double Latitude
        {
            get { return coordinates.Latitude; }
        }

        public double Longtitude
        {
            get { return coordinates.Longtitude; }
        }

        public string Type
        {
            get { return coordinates.Type; }
            set { coordinates.Type = value; }
        }
    }
}
