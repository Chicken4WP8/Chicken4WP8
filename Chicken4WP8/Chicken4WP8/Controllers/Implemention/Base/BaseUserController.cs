using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
        public IStorageService StorageService { get; set; }

        public BaseUserController()
        { }

        public async Task<IUserModel> ShowAsync(IDictionary<string, object> parameters)
        {
            var user = await tokens.Users.ShowAsync(parameters);
            if (user != null)
                return new UserModel(user);
            return null;
        }

        public async Task SetProfileImageAsync(IUserModel user)
        {
            string url = user.ProfileImageUrl;
            if (string.IsNullOrEmpty(url))
                return;
            string id = user.Id.Value + url;
            user.ProfileImageData = await GetImageAsync(id, url);
        }

        public async Task SetProfileBannerImageAsync(IUserModel user)
        {
            string url = user.UserProfileBannerImageUrl;
            if (string.IsNullOrEmpty(url))
                return;
            string id = user.Id.Value + url;
            user.ProfileBannerImageData = await GetImageAsync(id, url);
        }

        private async Task<byte[]> GetImageAsync(string id, string url)
        {
            var data = StorageService.GetCachedImage(id);
            if (data == null)
            {
                data = await base.DownloadImage(url);
                data = StorageService.AddOrUpdateImageCache(id, data);
            }
            return data;
        }
    }
}
