using System;
using System.Collections.Generic;

namespace Chicken4WP8.Controllers
{
    public interface IEntities
    {
        List<ISymbolEntity> HashTags { get; set; }
        List<IMediaEntity> Media { get; set; }
        List<ISymbolEntity> Symbols { get; set; }
        List<IUrlEntity> Urls { get; set; }
        List<IUserMentionEntity> UserMentions { get; set; }
    }

    public interface IEntity
    {
        EntityType EntityType { get; }
        int Index { get; set; }
        string DisplayText { get; }
    }

    public enum EntityType
    {
        None = 0,
        Media = 1,
        HashTag = 2,
        Symbol = 3,
        Url = 4,
        UserMention = 5,
    }

    public interface ISymbolEntity : IEntity
    {
        string Text { get; set; }
    }

    public interface IUrlEntity : IEntity
    {
        string DisplayUrl { get; set; }
        string TruncatedUrl { get; set; }
        Uri ExpandedUrl { get; set; }
        Uri Url { get; set; }
    }

    #region Media Entity
    public interface IMediaEntity : IUrlEntity
    {
        long Id { get; set; }
        Uri MediaUrl { get; set; }
        Uri MediaUrlHttps { get; set; }
        IMediaSizes Sizes { get; set; }
        long? SourceStatusId { get; set; }
        string Type { get; set; }
        byte[] ImageData { get; set; }
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
