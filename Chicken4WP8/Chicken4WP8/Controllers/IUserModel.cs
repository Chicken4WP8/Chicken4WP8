using System;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface IUserModel : IImageSource, INotifyPropertyChanged
    {
        long? Id { get; }
        string Name { get; }
        string ScreenName { get; }
        string Description { get; }
        DateTime CreatedAt { get; }
        string Location { get; }
        bool IsFollowing { get; }
        bool IsVerified { get; }
        bool IsPrivate { get; }
        bool IsTranslator { get; }
        Uri ProfileImageUrl { get; }
        Uri ProfileImageUrlHttps { get; }
    }
}
