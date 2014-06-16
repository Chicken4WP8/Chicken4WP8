using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipController : IFriendshipController
    {
        private readonly IFriendshipQueryExecutor _friendshipQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly IFactory<IRelationship> _relationshipFactory;
        private readonly IFactory<IRelationshipState> _relationshipStateFactory;
        private readonly IFactory<IFriendshipAuthorizations> _friendshipAuthorizationsFactory;

        public FriendshipController(
            IFriendshipQueryExecutor friendshipQueryExecutor,
            IUserFactory userFactory,
            IFriendshipFactory friendshipFactory,
            IFactory<IRelationship> relationshipFactory,
            IFactory<IRelationshipState> relationshipStateFactory,
            IFactory<IFriendshipAuthorizations> friendshipAuthorizationsFactory)
        {
            _friendshipQueryExecutor = friendshipQueryExecutor;
            _userFactory = userFactory;
            _friendshipFactory = friendshipFactory;
            _relationshipFactory = relationshipFactory;
            _relationshipStateFactory = relationshipStateFactory;
            _friendshipAuthorizationsFactory = friendshipAuthorizationsFactory;
        }

        // Get Users Requesting Friendship
        public IEnumerable<long> GetUserIdsRequestingFriendship()
        {
            return _friendshipQueryExecutor.GetUserIdsRequestingFriendship();
        }

        public IEnumerable<IUser> GetUsersRequestingFriendship()
        {
            var userIds = GetUserIdsRequestingFriendship();
            return _userFactory.GetUsersFromIds(userIds);
        }

        // Get Users You requested to follow
        public IEnumerable<long> GetUserIdsYouRequestedToFollow()
        {
            return _friendshipQueryExecutor.GetUserIdsYouRequestedToFollow();
        }

        public IEnumerable<IUser> GetUsersYouRequestedToFollow()
        {
            var userIds = GetUserIdsYouRequestedToFollow();
            return _userFactory.GetUsersFromIds(userIds);
        }

        // Create Friendship with
        public bool CreateFriendshipWith(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return CreateFriendshipWith(user.UserDTO);
        }

        public bool CreateFriendshipWith(IUserIdentifier userDTO)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(userDTO);
        }

        public bool CreateFriendshipWith(long userId)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(userId);
        }

        public bool CreateFriendshipWith(string userScreeName)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(userScreeName);
        }

        // Destroy Friendship with
        public bool DestroyFriendshipWith(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return DestroyFriendshipWith(user.UserDTO);
        }

        public bool DestroyFriendshipWith(IUserIdentifier userDTO)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(userDTO);
        }

        public bool DestroyFriendshipWith(long userId)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(userId);
        }

        public bool DestroyFriendshipWith(string userScreeName)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(userScreeName);
        }

        // Update Friendship Authorizations
        public bool UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return UpdateRelationshipAuthorizationsWith(user.UserDTO, retweetsEnabled, deviceNotifictionEnabled);
        }

        public bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(userDTO, friendshipAuthorizations);
        }

        public bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(userId, friendshipAuthorizations);
        }

        public bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(userScreenName, friendshipAuthorizations);
        }

        // Get Relationship (get between 2 users as there is no id for a relationship)
        public IRelationship GetRelationshipBetween(IUser sourceUser, IUser targetUser)
        {
            if (sourceUser == null || targetUser == null)
            {
                return null;
            }

            return GetRelationshipBetween(sourceUser.UserDTO, targetUser.UserDTO);
        }

        public IRelationship GetRelationshipBetween(IUserIdentifier sourceUserDTO, IUserIdentifier targetUserDTO)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserDTO, targetUserDTO);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        public IRelationship GetRelationshipBetween(long sourceUserId, long targetUserId)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserId, targetUserId);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        public IRelationship GetRelationshipBetween(long sourceUserId, string targetUserScreenName)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserId, targetUserScreenName);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        public IRelationship GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserScreenName, targetUserScreenName);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        public IRelationship GetRelationshipBetween(string sourceUserScreenName, long targetUserId)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserScreenName, targetUserId);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        // Get multiple relationships
        public Dictionary<IUser, IRelationshipState> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> targetUsers)
        {
            if (targetUsers == null)
            {
                return null;
            }

            var relationshipStates = GetRelationshipStatesWith(targetUsers.Select(x => x.UserDTO).ToList());
            var userRelationshipState = new Dictionary<IUser, IRelationshipState>();

            foreach (var targetUser in targetUsers)
            {
                var userRelationship = relationshipStates.FirstOrDefault(x => x.TargetId == targetUser.Id ||
                                                                              x.TargetScreenName == targetUser.ScreenName);
                userRelationshipState.Add(targetUser, userRelationship);
            }

            return userRelationshipState;
        }

        public IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<IUser> targetUsers)
        {
            if (targetUsers == null)
            {
                return null;
            }

            return GetRelationshipStatesWith(targetUsers.Select(x => x.UserDTO).ToList());
        }

        public IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<IUserIdentifier> targetUsersDTO)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipStatesWith(targetUsersDTO);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        public IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<long> targetUsersId)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipStatesWith(targetUsersId);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        public IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<string> targetUsersScreenName)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipStatesWith(targetUsersScreenName);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        // Generate From DTO
        public IRelationship GenerateRelationshipFromRelationshipDTO(IRelationshipDTO relationshipDTO)
        {
            if (relationshipDTO == null)
            {
                return null;
            }

            var relationshipParameter = _relationshipFactory.GenerateParameterOverrideWrapper("relationshipDTO", relationshipDTO);
            return _relationshipFactory.Create(relationshipParameter);
        }

        public IEnumerable<IRelationship> GenerateRelationshipsFromRelationshipsDTO(IEnumerable<IRelationshipDTO> relationshipDTOs)
        {
            if (relationshipDTOs == null)
            {
                return null;
            }

            return relationshipDTOs.Select(GenerateRelationshipFromRelationshipDTO).ToList();
        }

        // Generate Relationship state from DTO
        public IRelationshipState GenerateRelationshipStateFromRelationshipStateDTO(IRelationshipStateDTO relationshipStateDTO)
        {
            if (relationshipStateDTO == null)
            {
                return null;
            }

            var relationshipStateParameter = _relationshipFactory.GenerateParameterOverrideWrapper("relationshipStateDTO", relationshipStateDTO);
            return _relationshipStateFactory.Create(relationshipStateParameter);
        }

        public List<IRelationshipState> GenerateRelationshipStatesFromRelationshipStatesDTO(IEnumerable<IRelationshipStateDTO> relationshipStateDTOs)
        {
            if (relationshipStateDTOs == null)
            {
                return null;
            }

            return relationshipStateDTOs.Select(GenerateRelationshipStateFromRelationshipStateDTO).ToList();
        }

        // Generate RelationshipAuthorizations
        public IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool deviceNotificationEnabled)
        {
            var friendshipAuthorization = _friendshipAuthorizationsFactory.Create();

            friendshipAuthorization.RetweetsEnabled = retweetsEnabled;
            friendshipAuthorization.DeviceNotificationEnabled = deviceNotificationEnabled;

            return friendshipAuthorization;
        }

    }
}