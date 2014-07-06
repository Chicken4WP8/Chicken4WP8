using Chicken4WP8.Controllers;

namespace Chicken4WP8.Services.Interface
{
    public interface IImageCacheService
    {
        byte[] GetCachedProfileImage(IUserModel user);
        void AddProfileImageToCache(IUserModel user,byte[] data);
    }
}