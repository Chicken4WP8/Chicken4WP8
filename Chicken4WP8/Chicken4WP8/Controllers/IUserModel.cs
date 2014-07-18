using System;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface IUserModel : IImageSource, INotifyPropertyChanged
    {
        long? Id { get; set; }
        string Name { get; set; }
        string ScreenName { get; set; }
        string Description { get; set; }
        DateTime CreatedAt { get; set; }
        string Location { get; set; }
        bool IsFollowing { get; set; }
        bool IsVerified { get; set; }
        bool IsPrivate { get; set; }
        bool IsTranslator { get; set; }
        /// <summary>
        /// default, use bigger avatar
        /// </summary>
        string ProfileImageUrl { get; set; }
    }
}