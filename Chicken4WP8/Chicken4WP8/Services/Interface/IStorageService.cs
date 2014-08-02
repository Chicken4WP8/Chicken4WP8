using Chicken4WP8.Controllers;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.ViewModels.Base;

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

        ProfilePageNavigationArgs GetTempUser();
        void UpdateTempUser(ProfilePageNavigationArgs user);

        byte[] GetCachedImage(string id);
        byte[] AddOrUpdateImageCache(string id, byte[] data);
    }
}
