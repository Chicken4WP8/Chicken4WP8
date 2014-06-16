using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Streams
{
    public class TrackedStream : TwitterStream, ITrackedStream
    {
        public event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;

        protected readonly IStreamTrackManager<ITweet> _streamTrackManager;
        protected readonly IJsonObjectConverter _jsonObjectConverter;
        protected readonly ITweetFactory _tweetFactory;
        protected readonly ITwitterRequestGenerator _twitterRequestGenerator;
        protected readonly ISynchronousInvoker _synchronousInvoker;

        public override event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        public TrackedStream(
            IStreamTrackManager<ITweet> streamTrackManager,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            ITwitterRequestGenerator twitterRequestGenerator,
            ISynchronousInvoker synchronousInvoker)

            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper)
        {
            _streamTrackManager = streamTrackManager;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetFactory = tweetFactory;
            _twitterRequestGenerator = twitterRequestGenerator;
            _synchronousInvoker = synchronousInvoker;
        }

        public void StartStream(string url)
        {
            _synchronousInvoker.ExecuteSynchronously(StartStreamAsync(url));
        }

        public async Task StartStreamAsync(string url)
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
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var detectedTracksAndActions = _streamTrackManager.GetMatchingTracksAndActions(tweet.Text);
                var detectedTracks = detectedTracksAndActions.Select(x => x.Item1);
                if (detectedTracksAndActions.Any())
                {
                    this.Raise(MatchingTweetReceived, new MatchedTweetReceivedEventArgs(tweet, detectedTracks));
                }
            };

            await _streamResultGenerator.StartStreamAsync(generateTweetDelegate, generateWebRequest);
        }

        protected void RaiseJsonObjectReceived(string json)
        {
            this.Raise(JsonObjectReceived, new JsonObjectEventArgs(json));
        }

        public int TracksCount
        {
            get { return _streamTrackManager.TracksCount; }
        }

        public int MaxTracks
        {
            get { return _streamTrackManager.MaxTracks; }
        }

        public Dictionary<string, Action<ITweet>> Tracks
        {
            get { return _streamTrackManager.Tracks; }
        }

        public void AddTrack(string track, Action<ITweet> trackReceived = null)
        {
            _streamTrackManager.AddTrack(track, trackReceived);
        }

        public void RemoveTrack(string track)
        {
            _streamTrackManager.RemoveTrack(track);
        }

        public bool ContainsTrack(string track)
        {
            return _streamTrackManager.ContainsTrack(track);
        }

        public void ClearTracks()
        {
            _streamTrackManager.ClearTracks();
        }

        protected void RaiseMatchingTweetReceived(MatchedTweetReceivedEventArgs eventArgs)
        {
            this.Raise(MatchingTweetReceived, eventArgs);
        }
    }
}