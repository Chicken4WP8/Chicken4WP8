using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi
{
    public static class TweetListAsync
    {
        // Get Existing List
        public static async Task<ITweetList> GetExistingList(ITweetList list)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(list));
        }

        public static async Task<ITweetList> GetExistingList(ITweetListDTO listDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(listDTO));
        }

        public static async Task<ITweetList> GetExistingList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(listId));
        }

        public static async Task<ITweetList> GetExistingList(string slug, IUser user)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(slug, user));
        }

        public static async Task<ITweetList> GetExistingList(string slug, IUserIdentifier userDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(slug, userDTO));
        }

        public static async Task<ITweetList> GetExistingList(string slug, long userId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(slug, userId));
        }

        public static async Task<ITweetList> GetExistingList(string slug, string userScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetExistingList(slug, userScreenName));
        }

        // Get UserLists
        public static async Task<IEnumerable<ITweetList>> GetUserLists(IUser user, bool getOwnedListsFirst)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetUserLists(user, getOwnedListsFirst));
        }

        public static async Task<IEnumerable<ITweetList>> GetUserLists(IUserIdentifier userDTO, bool getOwnedListsFirst)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetUserLists(userDTO, getOwnedListsFirst));
        }

        public static async Task<IEnumerable<ITweetList>> GetUserLists(long userId, bool getOwnedListsFirst)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetUserLists(userId, getOwnedListsFirst));
        }

        public static async Task<IEnumerable<ITweetList>> GetUserLists(string userScreenName, bool getOwnedListsFirst)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetUserLists(userScreenName, getOwnedListsFirst));
        }

        // Create List
        public static async Task<ITweetList> CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.CreateList(name, privacyMode, description));
        }

        // Update List
        public static async Task<ITweetList> UpdateList(ITweetList tweetList, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(tweetList, parameters));
        }

        public static async Task<ITweetList> UpdateList(ITweetListDTO tweetListDTO, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(tweetListDTO, parameters));
        }

        public static async Task<ITweetList> UpdateList(long listId, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(listId, parameters));
        }

        public static async Task<ITweetList> UpdateList(string slug, IUser owner, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(slug, owner, parameters));
        }

        public static async Task<ITweetList> UpdateList(string slug, IUserIdentifier ownerDTO, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(slug, ownerDTO, parameters));
        }

        public static async Task<ITweetList> UpdateList(string slug, long ownerId, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(slug, ownerId, parameters));
        }

        public static async Task<ITweetList> UpdateList(string slug, string ownerScreenName, IListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.UpdateList(slug, ownerScreenName, parameters));
        }

        // Destroy List
        public static async Task<bool> DestroyList(ITweetList tweetList)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(tweetList));
        }

        public static async Task<bool> DestroyList(ITweetListDTO tweetListDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(tweetListDTO));
        }

        public static async Task<bool> DestroyList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(listId));
        }

        public static async Task<bool> DestroyList(string slug, IUser owner)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(slug, owner));
        }

        public static async Task<bool> DestroyList(string slug, IUserDTO ownerDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(slug, ownerDTO));
        }

        public static async Task<bool> DestroyList(string slug, long ownerId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(slug, ownerId));
        }

        public static async Task<bool> DestroyList(string slug, string ownerScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.DestroyList(slug, ownerScreenName));
        }

        // Get Tweets from List
        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(ITweetList tweetList)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(tweetList));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(ITweetListDTO tweetListDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(tweetListDTO));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(listId));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUser owner)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(slug, owner));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier ownerDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(slug, ownerDTO));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(slug, ownerScreenName));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetTweetsFromList(slug, ownerId));
        }

        // Get Members of List
        public static async Task<IEnumerable<IUser>> GetMembersOfList(ITweetList tweetList, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(tweetList, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(ITweetListDTO tweetListDTO, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(tweetListDTO, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(listId, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUser owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(slug, owner, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUserIdentifier ownerDTO, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(slug, ownerDTO, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(slug, ownerScreenName, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TweetList.GetMembersOfList(slug, ownerId, maxNumberOfUsersToRetrieve));
        }
    }
}
