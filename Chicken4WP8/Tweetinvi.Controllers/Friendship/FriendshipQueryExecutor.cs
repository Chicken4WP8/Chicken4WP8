using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipQueryExecutor
    {
        IEnumerable<long> GetUserIdsRequestingFriendship();
        IEnumerable<long> GetUserIdsYouRequestedToFollow();

        // Create Friendship
        bool CreateFriendshipWith(IUserIdentifier userDTO);
        bool CreateFriendshipWith(long userId);
        bool CreateFriendshipWith(string userScreenName);

        // Destroy Friendship
        bool DestroyFriendshipWith(IUserIdentifier userDTO);
        bool DestroyFriendshipWith(long userId);
        bool DestroyFriendshipWith(string userScreenName);

        // Update Friendship Authorization
        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations);
        bool UpdateRelationshipAuthorizationsWith(long userId, IFriendshipAuthorizations friendshipAuthorizations);
        bool UpdateRelationshipAuthorizationsWith(string userScreenName, IFriendshipAuthorizations friendshipAuthorizations);

        // Get Existing Relationship
        IRelationshipDTO GetRelationshipBetween(IUserIdentifier sourceUserDTO, IUserIdentifier targetUserDTO);
        IRelationshipDTO GetRelationshipBetween(long sourceUserId, long targetUserId);
        IRelationshipDTO GetRelationshipBetween(long sourceUserId, string targetUserScreenName);
        IRelationshipDTO GetRelationshipBetween(string sourceUserScreenName, long targetUserId);
        IRelationshipDTO GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName);

        // Get Multiple Relationships
        IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<IUserIdentifier> targetUsersDTO);
        IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<long> targetUsersId);
        IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<string> targetUsersScreenName);
    }

    public class FriendshipQueryExecutor : IFriendshipQueryExecutor
    {
        private readonly IFriendshipQueryGenerator _friendshipQueryGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;

        public FriendshipQueryExecutor(
            IFriendshipQueryGenerator friendshipQueryGenerator,
            IUserQueryValidator userQueryValidator,
            ITwitterAccessor twitterAccessor,
            IUserQueryParameterGenerator userQueryParameterGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _friendshipQueryGenerator = friendshipQueryGenerator;
            _userQueryValidator = userQueryValidator;
        }

        public IEnumerable<long> GetUserIdsRequestingFriendship()
        {
            string query = _friendshipQueryGenerator.GetUserIdsRequestingFriendshipQuery();
            var userIdsDTO = _twitterAccessor.ExecuteCursorGETQuery<IIdsCursorQueryResultDTO>(query);

            if (userIdsDTO == null)
            {
                return null;
            }

            var userIdsDTOList = userIdsDTO.ToList();

            var userdIds = new List<long>();
            for (int i = 0; i < userIdsDTOList.Count; ++i)
            {
                userdIds.AddRange(userIdsDTOList.ElementAt(i).Ids);
            }

            return userdIds;
        }

        public IEnumerable<long> GetUserIdsYouRequestedToFollow()
        {
            string query = _friendshipQueryGenerator.GetUserIdsYouRequestedToFollowQuery();
            var userIdsDTO = _twitterAccessor.ExecuteCursorGETQuery<IIdsCursorQueryResultDTO>(query);

            if (userIdsDTO == null)
            {
                return null;
            }

            var userIdsDTOList = userIdsDTO.ToList();

            var userdIds = new List<long>();
            for (int i = 0; i < userIdsDTOList.Count; ++i)
            {
                userdIds.AddRange(userIdsDTOList.ElementAt(i).Ids);
            }

            return userdIds;
        }

        // Create Friendship
        public bool CreateFriendshipWith(IUserIdentifier userDTO)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool CreateFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool CreateFriendshipWith(string userScreenName)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Destroy Friendship
        public bool DestroyFriendshipWith(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return false;
            }

            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool DestroyFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool DestroyFriendshipWith(string userScreenName)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Update Friendship Authorizations
        public bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return false;
            }

            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, friendshipAuthorizations);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UpdateRelationshipAuthorizationsWith(long userId, IFriendshipAuthorizations friendshipAuthorizations)
        {
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, friendshipAuthorizations);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UpdateRelationshipAuthorizationsWith(string userScreenName, IFriendshipAuthorizations friendshipAuthorizations)
        {
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userScreenName, friendshipAuthorizations);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Get Existing Relationship
        public IRelationshipDTO GetRelationshipBetween(IUserIdentifier sourceUserDTO, IUserIdentifier targetUserDTO)
        {
            string sourceParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(sourceUserDTO, "source_id", "source_screen_name");
            string targetParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(targetUserDTO, "target_id", "target_screen_name");
            string query = String.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);

            return _twitterAccessor.ExecuteGETQuery<IRelationshipDTO>(query);
        }

        public IRelationshipDTO GetRelationshipBetween(long sourceUserId, long targetUserId)
        {
            string sourceParameter = _userQueryParameterGenerator.GenerateUserIdParameter(sourceUserId);
            string targetParameter = _userQueryParameterGenerator.GenerateUserIdParameter(targetUserId);
            string query = String.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);

            return _twitterAccessor.ExecuteGETQuery<IRelationshipDTO>(query);
        }

        public IRelationshipDTO GetRelationshipBetween(long sourceUserId, string targetUserScreenName)
        {
            string sourceParameter = _userQueryParameterGenerator.GenerateUserIdParameter(sourceUserId);
            string targetParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(targetUserScreenName);
            string query = String.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);

            return _twitterAccessor.ExecuteGETQuery<IRelationshipDTO>(query);
        }

        public IRelationshipDTO GetRelationshipBetween(string sourceUserScreenName, long targetUserId)
        {
            string sourceParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(sourceUserScreenName);
            string targetParameter = _userQueryParameterGenerator.GenerateUserIdParameter(targetUserId);
            string query = String.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);

            return _twitterAccessor.ExecuteGETQuery<IRelationshipDTO>(query);
        }

        public IRelationshipDTO GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            string sourceParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(sourceUserScreenName);
            string targetParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(targetUserScreenName);
            string query = String.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);

            return _twitterAccessor.ExecuteGETQuery<IRelationshipDTO>(query);
        }

        // Get Relationship with
        public IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<IUserIdentifier> targetUsersDTO)
        {
            string userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserDTOParameter(targetUsersDTO);
            string query = String.Format(Resources.Friendship_GetRelationships, userIdsAndScreenNameParameter);

            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);
        }

        public IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<long> targetUsersId)
        {
            string userIds = _userQueryParameterGenerator.GenerateListOfIdsParameter(targetUsersId);
            string userIdsParameter = String.Format("user_id={0}", userIds);
            string query = String.Format(Resources.Friendship_GetRelationships, userIdsParameter);

            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);
        }

        public IEnumerable<IRelationshipStateDTO> GetRelationshipStatesWith(IEnumerable<string> targetUsersScreenName)
        {
            string userScreenNames = _userQueryParameterGenerator.GenerateListOfScreenNameParameter(targetUsersScreenName);
            string userScreenNamesParameter = String.Format("screen_name={0}", userScreenNames);
            string query = String.Format(Resources.Friendship_GetRelationships, userScreenNamesParameter);

            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);
        }
    }
}