using System.Threading.Tasks;
using Chicken4WP8.Models;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;
using CoreTweet;

namespace Chicken4WP8.Services.Implemention
{
    public class OAuthService : IOAuthService
    {
        private OAuth.OAuthSession session;
        private Tokens tokens;

        public async Task<OAuthSessionModel> AuthorizeAsync(string consumerKey, string consumerSecret)
        {
            session = await OAuth.AuthorizeAsync(consumerKey, consumerSecret);
            return new OAuthSessionModel(session.AuthorizeUri)
            {
                ConsumerKey = session.ConsumerKey,
                ConsumerSecret = session.ConsumerSecret,
                RequestToken = session.RequestToken,
                RequestTokenSecret = session.RequestTokenSecret
            };
        }

        public async Task<OAuthSetting> GetTokensAsync(string pinCode)
        {
            tokens = await OAuth.GetTokensAsync(session, pinCode);
            return new BaseOAuthSetting
            {
                ConsumerKey = session.ConsumerKey,
                ConsumerSecret = session.ConsumerSecret,
                AccessToken = tokens.AccessToken,
                AccessTokenSecret = tokens.AccessTokenSecret
            };
        }

        public async Task<IUserModel> VerifyCredentialsAsync()
        {
            var user = await tokens.Account.VerifyCredentialsAsync();
            return new UserModel(user);
        }
    }
}
