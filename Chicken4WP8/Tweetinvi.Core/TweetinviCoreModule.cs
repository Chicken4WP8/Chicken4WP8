using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Core
{
    public class TweetinviCoreModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviCoreModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterGeneric(typeof(IFactory<>), typeof(Factory<>));
            _container.RegisterType<ITaskFactory, TaskFactory>();
            _container.RegisterType<ISynchronousInvoker, SynchronousInvoker>();
        }
    }
}