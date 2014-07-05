using System;
using System.Collections.Generic;

namespace Chicken4WP8.Controllers
{
    public interface IEntities
    {
        IList<ISymbolEntity> HashTags { get; set; }
        IList<IMediaEntity> Media { get; set; }
        IList<ISymbolEntity> Symbols { get; set; }
        IList<IUrlEntity> Urls { get; set; }
        IList<IUserMentionEntity> UserMentions { get; set; }
    }

    public interface IEntity
    {
        int[] Indices { get; set; }
    }

    public interface ISymbolEntity : IEntity
    {
        string Text { get; set; }
    }

    #region Media Entity
    public interface IMediaEntity : IEntity
    {
        long Id { get; set; }
        Uri MediaUrl { get; set; }
        Uri MediaUrlHttps { get; set; }
        IMediaSizes Sizes { get; set; }
        long? SourceStatusId { get; set; }
        string Type { get; set; }
    }

    public interface IMediaSizes
    {
        IMediaSize Large { get; set; }
        IMediaSize Medium { get; set; }
        IMediaSize Small { get; set; }
        IMediaSize Thumb { get; set; }
    }

    public interface IMediaSize
    {
        int Height { get; set; }
        string Resize { get; set; }
        int Width { get; set; }
    }
    #endregion

    public interface IUrlEntity : IEntity
    {
        string DisplayUrl { get; set; }
        Uri ExpandedUrl { get; set; }
        Uri Url { get; set; }
    }

    public interface IUserMentionEntity : IEntity
    {
        long Id { get; set; }
        string Name { get; set; }
        string ScreenName { get; set; }
    }

    public interface ICoordinates
    {
        double Latitude { get; }
        double Longtitude { get; }
        string Type { get; set; }
    }
}
