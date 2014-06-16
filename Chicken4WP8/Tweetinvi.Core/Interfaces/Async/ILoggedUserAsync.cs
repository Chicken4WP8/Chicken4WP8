using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ILoggedUserAsync
    {
        Task<IEnumerable<IMessage>> GetLatestMessagesReceivedAsync(int count = 40);
        Task<IEnumerable<IMessage>> GetLatestMessagesSentAsync(int maximumMessages = 40);
        Task<IMessage> PublishMessageAsync(IMessage message);

        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(int count = 40);
        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(ITimelineRequestParameters timelineRequestParameters);
        Task<IEnumerable<IMention>> GetMentionsTimelineAsync(int count = 40);

        Task<IEnumerable<IRelationshipState>> GetRelationshipStatesWithAsync(IEnumerable<IUser> users);
        Task<Dictionary<IUser, IRelationshipState>> GetRelationshipStatesAssociatedWithAsync(IEnumerable<IUser> users);

        Task<IEnumerable<IUser>> GetUsersRequestingFriendshipAsync();
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollowAsync();

        Task<bool> FollowUserAsync(IUser user);
        Task<bool> UnFollowUserAsync(IUser user);
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(IUser user, bool retweetsEnabled, bool deviceNotificationsEnabled);

        Task<IEnumerable<ISavedSearch>> GetSavedSearchesAsync();

        Task<IEnumerable<IUser>> GetBlockedUsersAsync(bool createBlockUsers = true, bool createBlockedUsersIds = true);
        Task<IEnumerable<long>> GetBlockedUsersIdsAsync(bool createBlockedUsersIds = true);
        Task<IEnumerable<ISuggestedUserList>> GetSuggestedUserListAsync(bool createSuggestedUserList = true);
        Task<IAccountSettings> GetAccountSettingsAsync();
    }
}