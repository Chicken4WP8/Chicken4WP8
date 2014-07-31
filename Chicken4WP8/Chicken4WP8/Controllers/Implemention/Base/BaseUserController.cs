using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
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

        public async Task LookupFriendshipAsync(IUserModel user)
        {
            var option = Const.GetDictionary();
            option.Add(Const.USER_ID, user.Id);
            var friendships = await tokens.Friendships.LookupAsync(option);
            if (friendships != null && friendships.Count != 0
                && friendships[0].Connections != null && friendships[0].Connections.Length != 0)
            {
                var connections = friendships[0].Connections.Select(c => c.ToLower()).ToList();

                user.IsFollowing = connections.Contains(Const.FOLLOWING);
                user.IsFollowedBy = connections.Contains(Const.FOLLOWED_BY);
            }
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
