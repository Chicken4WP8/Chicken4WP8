using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface IUserModel : INotifyPropertyChanged
    {
        long? Id { get; set; }
        string Name { get; set; }
        string ScreenName { get; set; }
        DateTime CreatedAt { get; set; }
        string Description { get; set; }
        IUserEntities Entities { get; set; }
        bool IsFollowing { get; set; }
        bool IsFollowedBy { get; set; }
        bool IsVerified { get; set; }
        bool IsPrivate { get; set; }
        bool IsTranslator { get; set; }
        /// <summary>
        /// default, use bigger avatar
        /// </summary>
        string ProfileImageUrl { get; set; }
        byte[] ProfileImageData { get; set; }
        string UserProfileBannerImageUrl { get; set; }
        byte[] ProfileImageBannerImageData { get; set; }
        string Location { get; set; }
        string Url { get; set; }
        int TweetsCount { get; set; }
        int FollowingCount { get; set; }
        int FollowersCount { get; set; }
        int FavoritesCount { get; set; }
        List<IEntity> ParsedEntities { get; }
        bool IsProfileDetail { get; set; }
    }
}