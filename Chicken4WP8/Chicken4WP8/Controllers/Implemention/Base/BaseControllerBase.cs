using Chicken4WP8.Models.Setting;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public abstract class BaseControllerBase : ControllerBase
    {
        protected Tokens tokens;

        public BaseControllerBase()
        {
            var oauth = App.UserSetting.OAuthSetting as BaseOAuthSetting;
            tokens = Tokens.Create(oauth.ConsumerKey, oauth.ConsumerSecret, oauth.AccessToken, oauth.AccessTokenSecret);
        }
    }
}
