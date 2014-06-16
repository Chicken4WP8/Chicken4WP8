using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Injectinvi;

namespace Tweetinvi
{
    public class TweetinviModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ITweetinviContainer, AutofacContainer>(RegistrationLifetime.InstancePerApplication);
        }
    }
}