using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IUserController : IUserControllerAsync
    {
        // Friends
        IEnumerable<long> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000);

        IEnumerable<IUser> GetFriends(IUser user, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250);

        // Followers
        IEnumerable<long> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        IEnumerable<IUser> GetFollowers(IUser user, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250);

        // Favourites
        IEnumerable<ITweet> GetFavouriteTweets(IUser user, int maxFavouritesToRetrieve = 40);
        IEnumerable<ITweet> GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40);
        IEnumerable<ITweet> GetFavouriteTweets(long userId, int maxFavouritesToRetrieve = 40);
        IEnumerable<ITweet> GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve = 40);

        // Block User
        bool BlockUser(IUser user);
        bool BlockUser(IUserIdentifier userDTO);
        bool BlockUser(long userId);
        bool BlockUser(string userScreenName);

        // Stream Profile Image
        Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal);
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
    }
}