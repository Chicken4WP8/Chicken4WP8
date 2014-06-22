using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.User
{
    /// <summary>
    /// Reason for change : Twitter changes the operation exposed on its REST API
    /// </summary>
    public class UserController : IUserController
    {
        private readonly IUserQueryExecutor _userQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
        }

        #region sync
        #region Friend Ids
        public IEnumerable<long> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFriendIds(user.UserDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(userDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(userId, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(userScreenName, maxFriendsToRetrieve);
        }
        #endregion

        #region Friends
        public IEnumerable<IUser> GetFriends(IUser user, int maxFriendsToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFriends(user.UserDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<IUser> GetFriends(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userDTO, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }

        public IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userId, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }

        public IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userScreenName, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }
        #endregion

        #region Follower Ids
        public IEnumerable<long> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFollowerIds(user.UserDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(userDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(userId, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(userScreenName, maxFollowersToRetrieve);
        }
        #endregion

        #region Followers
        public IEnumerable<IUser> GetFollowers(IUser user, int maxFollowersToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFollowers(user.UserDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<IUser> GetFollowers(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userDTO, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }

        public IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userId, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }

        public IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userScreenName, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }
        #endregion

        #region Favorites
        public IEnumerable<ITweet> GetFavouriteTweets(IUser user, int maxFavouritesToRetrieve = 40)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFavouriteTweets(user.UserDTO, maxFavouritesToRetrieve);
        }

        public IEnumerable<ITweet> GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40)
        {
            var favoriteTweetsDTO = _userQueryExecutor.GetFavouriteTweets(userDTO, maxFavouritesToRetrieve);
            return _tweetFactory.GenerateTweetsFromDTO(favoriteTweetsDTO);
        }

        public IEnumerable<ITweet> GetFavouriteTweets(long userId, int maxFavouritesToRetrieve = 40)
        {
            var favoriteTweetsDTO = _userQueryExecutor.GetFavouriteTweets(userId, maxFavouritesToRetrieve);
            return _tweetFactory.GenerateTweetsFromDTO(favoriteTweetsDTO);
        }

        public IEnumerable<ITweet> GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve = 40)
        {
            var favoriteTweetsDTO = _userQueryExecutor.GetFavouriteTweets(userScreenName, maxFavouritesToRetrieve);
            return _tweetFactory.GenerateTweetsFromDTO(favoriteTweetsDTO);
        }
        #endregion

        #region Block User
        public bool BlockUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return BlockUser(user.UserDTO);
        }

        public bool BlockUser(IUserIdentifier userDTO)
        {
            return _userQueryExecutor.BlockUser(userDTO);
        }

        public bool BlockUser(long userId)
        {
            return _userQueryExecutor.BlockUser(userId);
        }

        public bool BlockUser(string userScreenName)
        {
            return _userQueryExecutor.BlockUser(userScreenName);
        }
        #endregion

        #region Profile Image
        public Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetProfileImageStream(user.UserDTO, imageSize);
        }

        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return _userQueryExecutor.GetProfileImageStream(userDTO, imageSize);
        }
        #endregion
        #endregion

        #region async
        #region Friend Ids
        public async Task<IEnumerable<long>> GetFriendIdsAsync(IUser user, int maxFriendsToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            };
            return await _userQueryExecutor.GetFriendIdsAsync(user.UserDTO, maxFriendsToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFriendIdsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFriendIdsAsync(userDTO, maxFriendsToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFriendIdsAsync(long userId, int maxFriendsToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFriendIdsAsync(userId, maxFriendsToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFriendIdsAsync(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFriendIdsAsync(userScreenName, maxFriendsToRetrieve);
        }
        #endregion

        #region Friends
        public async Task<IEnumerable<IUser>> GetFriendsAsync(IUser user, int maxFriendsToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }
            return await GetFriendsAsync(user.UserDTO, maxFriendsToRetrieve);
        }

        public async Task<IEnumerable<IUser>> GetFriendsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250)
        {
            var friendIds = await GetFriendIdsAsync(userDTO, maxFriendsToRetrieve);
            return /*await*/  _userFactory.GetUsersFromIds(friendIds);
        }

        public async Task<IEnumerable<IUser>> GetFriendsAsync(long userId, int maxFriendsToRetrieve = 250)
        {
            var friendIds = await GetFriendIdsAsync(userId, maxFriendsToRetrieve);
            return /*await*/  _userFactory.GetUsersFromIds(friendIds);
        }

        public async Task<IEnumerable<IUser>> GetFriendsAsync(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            var friendIds = await GetFriendIdsAsync(userScreenName, maxFriendsToRetrieve);
            return /*await*/  _userFactory.GetUsersFromIds(friendIds);
        }
        #endregion

        #region Follower Ids
        public async Task<IEnumerable<long>> GetFollowerIdsAsync(IUser user, int maxFollowersToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return await GetFollowerIdsAsync(user.UserDTO, maxFollowersToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFollowerIdsAsync(userDTO, maxFollowersToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(long userId, int maxFollowersToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFollowerIdsAsync(userId, maxFollowersToRetrieve);
        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return await _userQueryExecutor.GetFollowerIdsAsync(userScreenName, maxFollowersToRetrieve);
        }
        #endregion

        #region Followers
        public async Task<IEnumerable<IUser>> GetFollowersAsync(IUser user, int maxFollowersToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return await GetFollowersAsync(user.UserDTO, maxFollowersToRetrieve);
        }

        public async Task<IEnumerable<IUser>> GetFollowersAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIdsAsync(userDTO, maxFollowersToRetrieve);
            return /*await*/ _userFactory.GetUsersFromIds(followerIds);
        }

        public async Task<IEnumerable<IUser>> GetFollowersAsync(long userId, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIdsAsync(userId, maxFollowersToRetrieve);
            return /*await*/ _userFactory.GetUsersFromIds(followerIds);
        }

        public async Task<IEnumerable<IUser>> GetFollowersAsync(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIdsAsync(userScreenName, maxFollowersToRetrieve);
            return /*await*/ _userFactory.GetUsersFromIds(followerIds);
        }
        #endregion

        #region Favorites
        public async Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(IUser user, int maxFavouritesToRetrieve = 40)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return await GetFavouriteTweetsAsync(user.UserDTO, maxFavouritesToRetrieve);
        }

        public async Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40)
        {
            var favoriteTweetsDTO = await _userQueryExecutor.GetFavouriteTweets(userDTO, maxFavouritesToRetrieve);
            return await _tweetFactory.GenerateTweetsFromDTO(favoriteTweetsDTO);
        }

        public async Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(long userId, int maxFavouritesToRetrieve = 40)
        {

        }

        public async Task<IEnumerable<ITweet>> GetFavouriteTweetsAsync(string userScreenName, int maxFavouritesToRetrieve = 40)
        {

        }
        #endregion

        #region Block User
        public async Task<bool> BlockUserAsync(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return await BlockUserAsync(user.UserDTO);
        }

        public async Task<bool> BlockUserAsync(IUserIdentifier userDTO)
        {
            return await _userQueryExecutor.BlockUserAsync(userDTO);
        }

        public async Task<bool> BlockUserAsync(long userId)
        {
            return await _userQueryExecutor.BlockUserAsync(userId);
        }

        public async Task<bool> BlockUserAsync(string userScreenName)
        {
            return await _userQueryExecutor.BlockUserAsync(userScreenName);
        }
        #endregion

        #region Profile Image
        public async Task<Stream> GetProfileImageStreamAsync(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }
            return await GetProfileImageStreamAsync(user.UserDTO, imageSize);
        }

        public async Task<Stream> GetProfileImageStreamAsync(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return await _userQueryExecutor.GetProfileImageStreamAsync(userDTO, imageSize);
        }
        #endregion
        #endregion
    }
}