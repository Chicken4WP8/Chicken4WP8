using Chicken4WP8.Models.Setting;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implementation.Custom
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected Tokens tokens;

        public CustomControllerBase()
        {
            var oauth = App.UserSetting.OAuthSetting as CustomOAuthSetting;
            tokens = Tokens.Create(oauth.ConsumerKey, oauth.ConsumerSecret, oauth.AccessToken, oauth.AccessTokenSecret);
        }
    }
}
