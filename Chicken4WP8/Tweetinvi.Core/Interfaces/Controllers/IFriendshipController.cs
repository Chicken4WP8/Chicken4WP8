using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IFriendshipController
    {
        IEnumerable<long> GetUserIdsRequestingFriendship();
        IEnumerable<IUser> GetUsersRequestingFriendship();

        IEnumerable<long> GetUserIdsYouRequestedToFollow();
        IEnumerable<IUser> GetUsersYouRequestedToFollow();

        // Create Friendship with
        bool CreateFriendshipWith(IUser user);
        bool CreateFriendshipWith(IUserIdentifier userDTO);
        bool CreateFriendshipWith(long userId);
        bool CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        bool DestroyFriendshipWith(IUser user);
        bool DestroyFriendshipWith(IUserIdentifier userDTO);
        bool DestroyFriendshipWith(long userId);
        bool DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        bool UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled);

        // Relationship
        IRelationship GetRelationshipBetween(IUser sourceUser, IUser targetUser);
        IRelationship GetRelationshipBetween(IUserIdentifier sourceUserDTO, IUserIdentifier targetUserDTO);

        IRelationship GetRelationshipBetween(long sourceUserId, long targetUserId);
        IRelationship GetRelationshipBetween(long sourceUserId, string targetUserScreenName);

        IRelationship GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName);
        IRelationship GetRelationshipBetween(string sourceUserScreenName, long targetUserId);

        // Get Relationships between the current user and a list of users
        Dictionary<IUser, IRelationshipState> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> targetUsers);

        IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<IUser> targetUsers);
        IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<IUserIdentifier> targetUsersDTO);
        IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<long> targetUsersId);
        IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<string> targetUsersScreenName);
    }
}