﻿using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Model;

namespace Tweetinvi.Streams
{
    public class StreaminviModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public StreaminviModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IUserStream, UserStream>();
            _container.RegisterType<ITweetStream, TweetStream>();
            _container.RegisterType<ISampleStream, SampleStream>();
            _container.RegisterType<ITrackedStream, TrackedStream>();
            _container.RegisterType<IFilteredStream, FilteredStream>();

            _container.RegisterType<IWarningMessage, WarningMessage>();
            _container.RegisterType<IWarningMessageTooManyFollowers, WarningMessageTooManyFollowers>();
            _container.RegisterType<IWarningMessageFallingBehind, WarningMessageFallingBehind>();

            _container.RegisterType<IStreamResultGenerator, StreamResultGenerator>();
            _container.RegisterGeneric(typeof(IStreamTrackManager<>), typeof(StreamTrackManager<>));
        }
    }
}