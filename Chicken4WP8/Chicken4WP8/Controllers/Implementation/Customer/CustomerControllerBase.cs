using Chicken4WP8.Models.Setting;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implementation.Customer
{
    public abstract class CustomerControllerBase : ControllerBase
    {
        protected Tokens tokens;

        public CustomerControllerBase()
        {
            var oauth = App.UserSetting.OAuthSetting as CustomerOAuthSetting;
            tokens = Tokens.Create(oauth.ConsumerKey, oauth.ConsumerSecret, oauth.AccessToken, oauth.AccessTokenSecret);
        }
    }
}
