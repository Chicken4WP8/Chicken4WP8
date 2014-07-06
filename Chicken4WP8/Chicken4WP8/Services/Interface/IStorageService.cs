using Chicken4WP8.Models.Setting;

namespace Chicken4WP8.Services.Interface
{
    public interface IStorageService
    {
        UserSetting GetCurrentUserSetting();
        void UpdateCurrentUserSetting(UserSetting setting);

        string GetCurrentLanguage();
        void UpdateLanguage(string name);

        //Tweet GetTempTweet();
        //void UpdateTempTweet(Tweet tweet);

        //User GetTempUser();
        //void UpdateTempUser(User user);

        byte[] GetCachedImage(string key, string id = null);
        void AddOrUpdateImageCache(string key, byte[] data, string id = null);
    }
}
