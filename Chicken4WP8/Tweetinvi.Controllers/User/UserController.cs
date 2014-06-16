﻿using System;
using System.Collections.Generic;
using System.IO;
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

        // Friend Ids
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

        // Friends
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

        // Follower Ids
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

        // Followers
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

        // Favourites
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

        // Block User
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

        // Stream Profile Image
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
    }
}