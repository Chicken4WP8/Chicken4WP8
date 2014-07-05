using System;
using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models;
using Chicken4WP8.Models.Setting;

namespace Chicken4WP8.Controllers.Implemention
{
    public class TwipOAuthController : IOAuthController
    {
        public Task<OAuthSessionModel> AuthorizeAsync(string consumerKey, string consumerSecret)
        {
            throw new NotImplementedException();
        }

        public Task<OAuthSetting> GetTokensAsync(string pinCode)
        {
            throw new NotImplementedException();
        }

        public Task<IUserModel> VerifyCredentialsAsync(OAuthSetting setting)
        {
            throw new NotImplementedException();
        }
    }
}
