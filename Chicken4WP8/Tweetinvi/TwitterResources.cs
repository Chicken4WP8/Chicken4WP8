using Tweetinvi.Core.Interfaces;

namespace Tweetinvi
{
    public class TwitterResources
    {
        private const string BASE_URL = "https://api.twitter.com/";
        private static readonly IResourcesManager resourcesManager;

        static TwitterResources()
        {
            resourcesManager = TweetinviContainer.Resolve<IResourcesManager>();
            BaseUrl = BASE_URL;
        }

        public static string BaseUrl
        {
            get { return resourcesManager.BaseUrl; }
            set { resourcesManager.BaseUrl = value; }
        }
    }
}
