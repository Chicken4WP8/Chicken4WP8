using Chicken4WP8.Controllers;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Services.Implemention
{
    public class ImageCacheService : IImageCacheService
    {
        public IStorageService StorageSerive { get; set; }

        public ImageCacheService()
        { }

        public byte[] GetCachedProfileImage(IUserModel user)
        {
            var data = StorageSerive.GetCachedImage(user.ProfileImageUrl.AbsoluteUri, user.Id.Value.ToString());
            return data;
        }

        public void AddProfileImageToCache(IUserModel user, byte[] data)
        {
            StorageSerive.AddOrUpdateImageCache(user.ProfileImageUrl.AbsoluteUri, data, user.Id.Value.ToString());
        }
    }
}
