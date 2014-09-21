using System.Collections.Generic;
using Chicken4WP8.Controllers;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Models.Tombstoning;

namespace Chicken4WP8.Services.Interface
{
    public interface IStorageService
    {
        UserSetting GetCurrentUserSetting();
        void UpdateCurrentUserSetting(UserSetting setting);

        string GetCurrentLanguage();
        void UpdateLanguage(string name);

        ITweetModel GetTempTweet();
        void UpdateTempTweet(ITweetModel tweet);

        string GetCachedUserName();
        void AddOrUpdateUserName(string name);

        IUserModel GetCachedUser(string name);
        void AddOrUpdateUserCache(IUserModel user);

        byte[] GetCachedImage(string id);
        byte[] AddOrUpdateImageCache(string id, byte[] data);

        long? GetSendDirectMessageSinceId();
        long? GetSendDirectMessageMaxId();
        long? GetReceivedDirectMessageSinceId();
        long? GetReceivedDirectMessageMaxId();

        void AddCachedDirectMessages(IEnumerable<IDirectMessageModel> messages);
        List<IDirectMessageModel> GetGroupedDirectMessages();

        string GetDirectMessageUserName();
        void AddOrUpdateDirectMessageUserName(string name);

        NewStatusModel GetTempNewStatus();
        void UpdateTempNewStatus(NewStatusModel status);

        T GetTombstoningData<T>(TombstoningType type, string id) where T : TombstoningDataBase;
        void AddOrUpdateTombstoningData<T>(TombstoningType type, string id, T data) where T : TombstoningDataBase;

        List<string> GetEmotions();
    }
}
