﻿using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Json
{
    public static class FriendshipJson
    {
        [ThreadStatic]
        private static IFriendshipJsonController _friendshipJsonController;
        public static IFriendshipJsonController FriendshipJsonController
        {
            get { return _friendshipJsonController; }
        }

        static FriendshipJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _friendshipJsonController = TweetinviContainer.Resolve<IFriendshipJsonController>();
        }

        public static IEnumerable<string> GetUserIdsRequestingFriendship()
        {
            return FriendshipJsonController.GetUserIdsRequestingFriendship();
        }

        public static IEnumerable<string> GetUserIdsYouRequestedToFollow()
        {
            return FriendshipJsonController.GetUserIdsYouRequestedToFollow();
        }

        public static string CreateFriendshipWith(IUser user)
        {
            return FriendshipJsonController.CreateFriendshipWith(user);
        }

        public static string CreateFriendshipWith(IUserIdentifier userDTO)
        {
            return FriendshipJsonController.CreateFriendshipWith(userDTO);
        }

        public static string CreateFriendshipWith(long userId)
        {
            return FriendshipJsonController.CreateFriendshipWith(userId);
        }

        public static string CreateFriendshipWith(string userScreeName)
        {
            return FriendshipJsonController.CreateFriendshipWith(userScreeName);
        }

        public static string DestroyFriendshipWith(IUser user)
        {
            return FriendshipJsonController.DestroyFriendshipWith(user);
        }

        public static string DestroyFriendshipWith(IUserIdentifier userDTO)
        {
            return FriendshipJsonController.DestroyFriendshipWith(userDTO);
        }

        public static string DestroyFriendshipWith(long userId)
        {
            return FriendshipJsonController.DestroyFriendshipWith(userId);
        }

        public static string DestroyFriendshipWith(string userScreeName)
        {
            return FriendshipJsonController.DestroyFriendshipWith(userScreeName);
        }

        public static string UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.DestroyFriendshipWith(user);
        }

        public static string UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(userDTO, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static string UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static string UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled);
        }
    }
}