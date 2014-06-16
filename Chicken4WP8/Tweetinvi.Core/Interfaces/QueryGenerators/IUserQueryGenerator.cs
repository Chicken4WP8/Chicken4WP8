using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        // Friend Ids
        string GetFriendIdsQuery(IUserIdentifier userDTO, int maxFriendsToRetrieve);
        string GetFriendIdsQuery(long userId, int maxFriendsToRetrieve);
        string GetFriendIdsQuery(string screenName, int maxFriendsToRetrieve);

        // Followers Ids
        string GetFollowerIdsQuery(IUserIdentifier userDTO, int maxFollowersToRetrieve);
        string GetFollowerIdsQuery(long userId, int maxFollowersToRetrieve);
        string GetFollowerIdsQuery(string screenName, int maxFollowersToRetrieve);

        // Favourites
        string GetFavouriteTweetsQuery(IUserIdentifier userDTO, int maxFavoritesToRetrieve);
        string GetFavouriteTweetsQuery(long userId, int maxFavoritesToRetrieve);
        string GetFavouriteTweetsQuery(string screenName, int maxFavoritesToRetrieve);

        // Block User
        string GetBlockUserQuery(IUserIdentifier userDTO);
        string GetBlockUserQuery(long userId);
        string GetBlockUserQuery(string userScreenName);

        // Download Profile Image
        string DownloadProfileImageURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
        string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
    }
}