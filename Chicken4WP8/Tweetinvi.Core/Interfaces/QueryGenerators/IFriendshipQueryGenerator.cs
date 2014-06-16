using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IFriendshipQueryGenerator
    {
        string GetUserIdsRequestingFriendshipQuery();
        string GetUserIdsYouRequestedToFollowQuery();

        // Create Friendship
        string GetCreateFriendshipWithQuery(IUserIdentifier userDTO);
        string GetCreateFriendshipWithQuery(long userId);
        string GetCreateFriendshipWithQuery(string screenName);

        // Destroy Friendship
        string GetDestroyFriendshipWithQuery(IUserIdentifier userDTO);
        string GetDestroyFriendshipWithQuery(long userId);
        string GetDestroyFriendshipWithQuery(string screenName);

        // Update Friendship Authorization
        string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations);
        string GetUpdateRelationshipAuthorizationsWithQuery(long userId, IFriendshipAuthorizations friendshipAuthorizations);
        string GetUpdateRelationshipAuthorizationsWithQuery(string screenName, IFriendshipAuthorizations friendshipAuthorizations);
    }
}
