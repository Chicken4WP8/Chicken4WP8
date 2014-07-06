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
            var data = StorageSerive.GetCachedImage(user.Id.Value.ToString(), user.ProfileImageUrl.AbsoluteUri);
            return data;
        }

        public void AddProfileImageToCache(IUserModel user, byte[] data)
        {
            //user id stands for primary key in database,
            //when user updated profile image,
            //the profile image url changed,
            //but user id not.
            StorageSerive.AddOrUpdateImageCache(user.Id.Value.ToString(), data, user.ProfileImageUrl.AbsoluteUri);
        }
    }
}
