using Tweetinvi.Core.Interfaces;

namespace Tweetinvi.Resource
{
    public class ResourcesManager : IResourcesManager
    {
        private const string BASE_URL = "https://api.twitter.com/";
        private static string baseUrl = BASE_URL;

        static ResourcesManager()
        {
        }

        public string BaseUrl
        {
            get
            {
                return baseUrl;
            }
            set
            {
                baseUrl = value;
            }
        }

        #region OAuth
        public string OAuth_PINCode_CallbackURL
        {
            get { return "oob"; }
        }

        public string OAuthRequestAccessToken
        {
            get { return string.Format("{0}oauth/access_token", baseUrl); }
        }

        public string OAuthRequestToken
        {
            get { return string.Format("{0}oauth/request_token", baseUrl); }
        }

        public string OAuthTokenRequestRegex
        {
            get { return @"oauth_token=(?<oauth_token>(?:\w|\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\w)*)&oauth_callback_confirmed=(?<oauth_callback_confirmed>(?:\w)*)"; }
        }

        public string OAuthRequestAuthorize
        {
            get { return string.Format("{0}oauth/authorize", baseUrl); }
        }

        public string OAuthTokenAccessRegex
        {
            get { return @"oauth_token=(?<oauth_token>(?:\w|\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\w)*)&user_id=(?<user_id>(?:\d)*)&screen_name=(?<screen_name>(?:\w)*)"; }
        }

        public string OAuthToken_GetVerifierCode_Regex
        {
            get { return @"(?<appCallbackUrl>(?:\w|\-)*)[\?&]oauth_token=(?<oauth_token>(?:\w|\-)*)&oauth_verifier=(?<oauth_verifier>(?:\w|\-)*)"; }
        }

        public string OAuthRequestAccessTokenWithUserCredentials
        {
            get { return string.Format("{0}oauth/access_token?x_auth_password ={0}x_auth_password={1}&x_auth_mode=client_auth", baseUrl); }
        }
        #endregion
    }
}
