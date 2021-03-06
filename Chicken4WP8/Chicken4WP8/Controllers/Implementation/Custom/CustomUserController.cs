﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers.Implementation.Base;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implementation.Custom
{
    public class CustomUserController : CustomControllerBase, IUserController
    {
        public async Task<IUserModel> ShowAsync(IDictionary<string, object> parameters)
        {
            IUserModel model = null;
            if (!parameters.ContainsKey(Const.NEED_REFRESH))
                model = StorageService.GetCachedUser(parameters[Const.USER_SCREEN_NAME] as string);
            if (model != null)
                return model;
            var user = await tokens.Users.ShowAsync(parameters);
            if (user == null)
                return null;
            model = new UserModel(user);
            StorageService.AddOrUpdateCachedUser(model);
            return model;
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
            IFriendshipModel model = StorageService.GetCachedFriendship(user.ScreenName);
            if (model == null)
            {
                var option = Const.GetDictionary();
                option.Add(Const.USER_ID, user.Id);
                var friendships = await tokens.Friendships.LookupAsync(option);
                if (friendships != null && friendships.Count != 0)
                    model = new FriendshipModel(friendships[0]);
            }
            if (model == null)
                return;

            var connections = model.Connections.Select(c => c.ToLower()).ToList();

            user.IsFollowing = connections.Contains(Const.FOLLOWING);
            user.IsFollowedBy = connections.Contains(Const.FOLLOWED_BY);

            StorageService.AddOrUpdateCachedFriendship(model);
        }
    }
}
