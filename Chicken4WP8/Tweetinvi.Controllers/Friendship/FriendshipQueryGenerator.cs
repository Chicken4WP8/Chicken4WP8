using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Friendship
{
   public class FriendshipQueryGenerator : IFriendshipQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;

        public FriendshipQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
        }

        // Get Friendship
        public string GetUserIdsRequestingFriendshipQuery()
        {
            return Resources.Friendship_GetIncomingIds;
        }

        public string GetUserIdsYouRequestedToFollowQuery()
        {
            return Resources.Friendship_GetOutgoingIds;
        }

        // Create Friendship
        public string GetCreateFriendshipWithQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateCreateFriendshipQuery(userIdentifierParameter);
        }

        public string GetCreateFriendshipWithQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateCreateFriendshipQuery(userIdParameter);
        }

        public string GetCreateFriendshipWithQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateCreateFriendshipQuery(userScreenNameParameter);
        }

        private string GenerateCreateFriendshipQuery(string userIdentifierParameter)
        {
            return String.Format(Resources.Friendship_Create, userIdentifierParameter);
        }

        // Destroy Friendship
        public string GetDestroyFriendshipWithQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateDestroyFriendshipQuery(userIdentifierParameter);
        }

        public string GetDestroyFriendshipWithQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateDestroyFriendshipQuery(userIdParameter);
        }

        public string GetDestroyFriendshipWithQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateDestroyFriendshipQuery(userScreenNameParameter);
        }

        private string GenerateDestroyFriendshipQuery(string userIdentifierParameter)
        {
            return String.Format(Resources.Friendship_Destroy, userIdentifierParameter);
        }

        // Update Relationship
        public string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GetUpdateRelationshipAuthorizationQuery(userIdentifierParameter, friendshipAuthorizations);
        }

        public string GetUpdateRelationshipAuthorizationsWithQuery(long userId, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GetUpdateRelationshipAuthorizationQuery(userIdParameter, friendshipAuthorizations);
        }

        public string GetUpdateRelationshipAuthorizationsWithQuery(string screenName, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GetUpdateRelationshipAuthorizationQuery(userScreenNameParameter, friendshipAuthorizations);
        }

        private string GetUpdateRelationshipAuthorizationQuery(string userIdentifierParameter, IFriendshipAuthorizations friendshipAuthorizations)
        {
            return String.Format(Resources.Friendship_Update, friendshipAuthorizations.RetweetsEnabled,
                                                              friendshipAuthorizations.DeviceNotificationEnabled,
                                                              userIdentifierParameter);
        }
    }
}