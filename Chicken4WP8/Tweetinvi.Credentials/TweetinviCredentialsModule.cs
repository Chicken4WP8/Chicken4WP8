using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Credentials
{
    public class TweetinviCredentialsModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviCredentialsModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ITwitterAccessor, TwitterAccessor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ICredentialsAccessor, CredentialsAccessor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ICredentialsCreator, CredentialsCreator>();
            _container.RegisterType<IWebTokenCreator, WebTokenCreator>();
        }
    }
}