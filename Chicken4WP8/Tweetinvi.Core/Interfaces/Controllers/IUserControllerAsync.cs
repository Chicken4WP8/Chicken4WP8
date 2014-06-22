using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IUserControllerAsync
    {
        // Friends
        Task<IEnumerable<long>> GetFriendIdsAsync(IUser user, int maxFriendsToRetrieve = 5000);
        Task<IEnumerable<long>> GetFriendIdsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000);
        Task<IEnumerable<long>> GetFriendIdsAsync(long userId, int maxFriendsToRetrieve = 5000);
        Task<IEnumerable<long>> GetFriendIdsAsync(string userScreenName, int maxFriendsToRetrieve = 5000);

        Task<IEnumerable<IUser>> GetFriendsAsync(IUser user, int maxFriendsToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFriendsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFriendsAsync(long userId, int maxFriendsToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFriendsAsync(string userScreenName, int maxFriendsToRetrieve = 250);

        // Followers
        Task<IEnumerable<long>> GetFollowerIdsAsync(IUser user, int maxFollowersToRetrieve = 5000);
        Task<IEnumerable<long>> GetFollowerIdsAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000);
        Task<IEnumerable<long>> GetFollowerIdsAsync(long userId, int maxFollowersToRetrieve = 5000);
        Task<IEnumerable<long>> GetFollowerIdsAsync(string userScreenName, int maxFollowersToRetrieve = 5000);

        Task<IEnumerable<IUser>> GetFollowersAsync(IUser user, int maxFollowersToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFollowersAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFollowersAsync(long userId, int maxFollowersToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFollowersAsync(string userScreenName, int maxFollowersToRetrieve = 250);

        // Favourites
        Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(IUser user, int maxFavouritesToRetrieve = 40);
        Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40);
        Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(long userId, int maxFavouritesToRetrieve = 40);
        Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(string userScreenName, int maxFavouritesToRetrieve = 40);

        // Block User
        Task<bool> BlockUserAsync(IUser user);
        Task<bool> BlockUserAsync(IUserIdentifier userDTO);
        Task<bool> BlockUserAsync(long userId);
        Task<bool> BlockUserAsync(string userScreenName);

        // Stream Profile Image
        Task<Stream> GetProfileImageStreamAsync(IUser user, ImageSize imageSize = ImageSize.normal);
        Task<Stream> GetProfileImageStreamAsync(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
    }
}
