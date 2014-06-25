using Tweetinvi.Core.Interfaces;

namespace Tweetinvi.Resource
{
    public class ResourcesManager : IResourcesManager
    {
        static ResourcesManager()
        {
        }

        public ResourcesManager()
        {
            baseUrl = "https://api.twitter.com/";
        }

        private string baseUrl;
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
            get { return string.Format("{0}oauth/access_token", BaseUrl); }
        }

        public string OAuthRequestToken
        {
            get { return string.Format("{0}oauth/request_token", BaseUrl); }
        }

        public string OAuthTokenRequestRegex
        {
            get { return @"oauth_token=(?<oauth_token>(?:\w|\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\w)*)&oauth_callback_confirmed=(?<oauth_callback_confirmed>(?:\w)*)"; }
        }

        public string OAuthRequestAuthorize
        {
            get { return string.Format("{0}oauth/authorize", BaseUrl); }
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
            get { return string.Format("{0}oauth/access_token?x_auth_password ={0}x_auth_password={1}&x_auth_mode=client_auth", BaseUrl); }
        }

        public string TokenUser_GetCurrentUser
        {
            get { return string.Format("{0}1.1/account/verify_credentials.json", BaseUrl); }
        }
        #endregion

        public string User_GetUserFromId
        {
            get { return "https://api.twitter.com/1.1/users/show.json?user_id={0}"; }
        }

        public string User_GetUserFromName
        {
            get { return "https://api.twitter.com/1.1/users/show.json?screen_name={0}"; }
        }

        public string User_GetUsersFromIds
        {
            get { return "https://api.twitter.com/1.1/users/lookup.json?user_id={0}"; }
        }

        public string User_GetUsersFromNames
        {
            get { return "https://api.twitter.com/1.1/users/lookup.json?screen_name={0}"; }
        }
    }
}
