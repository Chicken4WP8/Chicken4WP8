using System;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Streams
{
    public class TweetStream : TwitterStream, ITweetStream
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITwitterRequestGenerator _twitterRequestGenerator;

        public event EventHandler<TweetReceivedEventArgs> TweetReceived;
        public override event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        public TweetStream(
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITweetFactory tweetFactory,
            ITwitterRequestGenerator twitterRequestGenerator)
            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper)
        {
            _tweetFactory = tweetFactory;
            _twitterRequestGenerator = twitterRequestGenerator;
        }

        public async Task StartStream(string url)
        {
            Func<HttpWebRequest> generateWebRequest = delegate
            {
                if (_filterLanguage != null)
                {
                    url = url.AddParameterToQuery("language", _filterLanguage);
                }

                return _twitterRequestGenerator.GetQueryWebRequest(url, HttpMethod.GET);
            };

            Action<string> generateTweetDelegate = json =>
            {
                this.Raise(JsonObjectReceived, new JsonObjectEventArgs(json));

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                this.Raise(TweetReceived, new TweetReceivedEventArgs(tweet));
            };

            await _streamResultGenerator.StartStreamAsync(generateTweetDelegate, generateWebRequest);
        }
    }
}