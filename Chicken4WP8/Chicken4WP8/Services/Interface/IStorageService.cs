using System.Collections.Generic;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;

namespace Chicken4WP8.Services.Interface
{
    public interface IStorageService
    {
        UserSetting GetCurrentUserSetting();
        //void UpdateCurrentUser(User user);
        
        object GetOAuthSetting(int id);
        //void UpdageCurrentSetting(ICredentialStore CredentialStore);

        string GetCurrentLanguage();
        void UpdateLanguage(string name);

        //Tweet GetTempTweet();
        //void UpdateTempTweet(Tweet tweet);

        //User GetTempUser();
        //void UpdateTempUser(User user);

        //byte[] GetCachedImage(string url);
        //void AddCachedImage(string url, byte[] data);
    }
}
