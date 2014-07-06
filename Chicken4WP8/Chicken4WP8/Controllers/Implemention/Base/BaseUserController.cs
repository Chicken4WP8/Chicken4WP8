using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
        public IImageCacheService ImageCacheService { get; set; }

        public BaseUserController()
        { }

        public async Task SetProfileImageStreamAsync(IUserModel user)
        {
            var data = ImageCacheService.GetCachedProfileImage(user);
            if (data == null)
            {
                data = await base.DownloadImage(user.ProfileImageUrl);
                ImageCacheService.AddProfileImageToCache(user, data);
            }
            base.SetImageFromBytes(user, data);
        }
    }
}
