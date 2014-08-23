using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
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
            user.ProfileImageData = await base.GetImageAsync(id, url);
        }

        public async Task SetProfileImageDetailAsync(IUserModel user)
        {
            string url = user.ProfileImageUrl;
            if (string.IsNullOrEmpty(url))
                return;
            url = url.Replace("_bigger", "_200x200");
            string id = user.Id.Value + url;
            user.ProfileImageData = await base.GetImageAsync(id, url);
        }

        public async Task SetProfileBannerImageAsync(IUserModel user)
        {
            string url = user.UserProfileBannerImageUrl;
            if (string.IsNullOrEmpty(url))
                return;
            string id = user.Id.Value + url;
            user.ProfileBannerImageData = await base.GetImageAsync(id, url);
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
    }
}
