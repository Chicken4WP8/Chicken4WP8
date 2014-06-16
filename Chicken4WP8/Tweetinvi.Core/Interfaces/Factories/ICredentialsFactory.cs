using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ICredentialsFactory
    {
        IOAuthCredentials CreateOAuthCredentials(string userAccessToken, string userAccessSecret, string consumerKey, string consumerSecret);
    }
}