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

        IUserModel GetTempUser();
        void UpdateTempUser(IUserModel user);

        byte[] GetCachedImage(string id);
        void AddOrUpdateImageCache(string id, byte[] data);
    }
}
