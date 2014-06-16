using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi
{
    public static class RateLimit
    {
        [ThreadStatic]
        private static IHelpController _helpController;
        public static IHelpController HelpController
        {
            get
            {
                if (_helpController == null)
                {
                    Initialize();
                }

                return _helpController;
            }
        }

        static RateLimit()
        {
            Initialize();
        }

        static void Initialize()
        {
            _helpController = TweetinviContainer.Resolve<IHelpController>();
        }

        public static ITokenRateLimits GetCurrentCredentialsRateLimits()
        {
            return HelpController.GetCurrentCredentialsRateLimits();
        }

        public static ITokenRateLimits GetCredentialsRateLimits(IOAuthCredentials credentials)
        {
            return HelpController.GetCredentialsRateLimits(credentials);
        }
    }
}