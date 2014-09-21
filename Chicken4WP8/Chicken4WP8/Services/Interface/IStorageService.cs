using System.Collections.Generic;
using Chicken4WP8.Controllers;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Models.Tombstoning;

namespace Chicken4WP8.Services.Interface
{
    public interface IStorageService
    {
        #region settings
        UserSetting GetCurrentUserSetting();
        void UpdateCurrentUserSetting(UserSetting setting);

        string GetCurrentLanguage();
        void UpdateLanguage(string name);
        #endregion

        #region temp data
        ITweetModel GetTempTweet();
        void UpdateTempTweetId(long tweetId);

        string GetTempUserName();
        void UpdateTempUserName(string name);

        string GetTempDirectMessageUserName();
        void UpdateTempDirectMessageUserName(string name);

        NewTweetModel GetTempNewTweet();
        void UpdateTempNewTweet(NewTweetModel tweet);
        #endregion

        #region cached data
        void AddCachedTweets(IEnumerable<ITweetModel> tweets);

        IUserModel GetCachedUser(string name);
        void AddOrUpdateCachedUser(IUserModel user);
        void AddCachedUsers(IEnumerable<IUserModel> users);

        IFriendshipModel GetCachedFriendship(string name);
        void AddOrUpdateCachedFriendship(IFriendshipModel friendship);

        byte[] GetCachedImage(string id);
        byte[] AddOrUpdateImageCache(string id, byte[] data);

        long? GetSendDirectMessageSinceId();
        long? GetSendDirectMessageMaxId();
        long? GetReceivedDirectMessageSinceId();
        long? GetReceivedDirectMessageMaxId();

        void AddCachedDirectMessages(IEnumerable<IDirectMessageModel> messages);
        List<IDirectMessageModel> GetGroupedDirectMessages();
        #endregion

        T GetTombstoningData<T>(TombstoningType type, string id) where T : TombstoningDataBase;
        void AddOrUpdateTombstoningData<T>(TombstoningType type, string id, T data) where T : TombstoningDataBase;

        List<string> GetEmotions();
    }
}
