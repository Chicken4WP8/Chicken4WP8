﻿using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.WebLogic
{
    public class TweetinviWebLogicModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviWebLogicModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IWebRequestExecutor, WebRequestExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterRequester, TwitterRequester>();
            _container.RegisterType<ITwitterRequestGenerator, TwitterRequestGenerator>();

            _container.RegisterType<IConsumerCredentials, ConsumerCredentials>();
            _container.RegisterType<ITemporaryCredentials, TemporaryCredentials>();
            _container.RegisterType<IOAuthCredentials, OAuthCredentials>();

            _container.RegisterType<IOAuthQueryParameter, OAuthQueryParameter>();
            _container.RegisterType<IOAuthWebRequestGenerator, OAuthWebRequestGenerator>();
            
            _container.RegisterType<IWebHelper, WebHelper>();
        }
    }
}