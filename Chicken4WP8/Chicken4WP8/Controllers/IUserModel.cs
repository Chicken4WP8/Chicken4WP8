using System;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface IUserModel : IImageSource, INotifyPropertyChanged
    {
        long? Id { get; set; }
        string Name { get; set; }
        string ScreenName { get; set; }
        DateTime CreatedAt { get; set; }        
        bool IsVerified { get; set; }
        bool IsPrivate { get; set; }
        bool IsTranslator { get; set; }
        /// <summary>
        /// default, use bigger avatar
        /// </summary>
        string ProfileImageUrl { get; set; }
    }

    public interface IFriendModel : IUserModel
    {
        string Description { get; set; }
    }

    public interface IProfileModel : IFriendModel
    {
        string UserProfileBannerImage { get; set; }
        string Location { get; set; }
        string Url { get; set; }
        IUserEntities UserEntities { get; set; }
        int TweetsCount { get; set; }
        int FollowingCount { get; set; }
        int FollowersCount { get; set; }
        int FavoritesCount { get; set; }
        bool IsFollowing { get; set; }
        bool IsFollowedBy { get; set; }
    }
}