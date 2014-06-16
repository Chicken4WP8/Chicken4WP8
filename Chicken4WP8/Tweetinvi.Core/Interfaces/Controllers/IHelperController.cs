using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IHelpController
    {
        ITokenRateLimits GetCurrentCredentialsRateLimits();
        ITokenRateLimits GetCredentialsRateLimits(IOAuthCredentials credentials);
        string GetTwitterPrivacyPolicy();
    }
}