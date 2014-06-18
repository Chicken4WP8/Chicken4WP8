using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;

namespace Tweetinvi.Resource
{
    public class TweetinviResourcesModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviResourcesModule(ITweetinviContainer container)
        {
            this._container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IResourcesManager, ResourcesManager>(RegistrationLifetime.InstancePerApplication);
        }
    }
}
