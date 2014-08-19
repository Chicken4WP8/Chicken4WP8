using Chicken4WP8.Controllers;
using Chicken4WP8.Models.Setting;

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
    }
}
