using Chicken4WP8.Entities;

namespace Chicken4WP8.Services.Interface
{
    public interface IStorageService
    {
        //User GetCurrentUser();
        //void UpdateCurrentUser(User user);

        ////IList<Setting> GetProxySettings();
        //ICredentialStore GetCurrentOAuthSetting();
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
