using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class SampleStream : TweetStream, ISampleStream
    {
        private readonly ISynchronousInvoker _synchronousInvoker;

        public SampleStream(
            IStreamResultGenerator streamResultGenerator, 
            IJsonObjectConverter jsonObjectConverter, 
            IJObjectStaticWrapper jObjectStaticWrapper, 
            ITweetFactory tweetFactory,
            ITwitterRequestGenerator twitterRequestGenerator,
            ISynchronousInvoker synchronousInvoker)
            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, tweetFactory, twitterRequestGenerator)
        {
            _synchronousInvoker = synchronousInvoker;
        }

        public void StartStream()
        {
            _synchronousInvoker.ExecuteSynchronously(StartStream(Resources.Stream_Sample));
        }

        public async Task StartStreamAsync()
        {
            await StartStream(Resources.Stream_Sample);
        }
    }
}