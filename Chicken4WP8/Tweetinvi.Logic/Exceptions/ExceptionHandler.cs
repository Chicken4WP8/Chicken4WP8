using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Exceptions;

namespace Tweetinvi.Logic.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IFactory<ITwitterException> _twitterExceptionUnityFactory;

        private readonly List<ITwitterException> _getExceptionInfos;
        public event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived;
        public bool SwallowWebExceptions { get; set; }

        public ExceptionHandler(IFactory<ITwitterException> twitterExceptionUnityFactory)
        {
            _twitterExceptionUnityFactory = twitterExceptionUnityFactory;
            _getExceptionInfos = new List<ITwitterException>();
            SwallowWebExceptions = true;
        }

        public IEnumerable<ITwitterException> ExceptionInfos
        {
            get { return _getExceptionInfos; }
        }

        public ITwitterException LastExceptionInfos
        {
            get { return _getExceptionInfos.LastOrDefault(); }
        }

        public TwitterException AddWebException(WebException webException, string url)
        {
            var webExceptionParameterOverride = _twitterExceptionUnityFactory.GenerateParameterOverrideWrapper("webException", webException);
            var urlParameterOverride = _twitterExceptionUnityFactory.GenerateParameterOverrideWrapper("url", url);
            var twitterException = _twitterExceptionUnityFactory.Create(webExceptionParameterOverride, urlParameterOverride);
            _getExceptionInfos.Add(twitterException);

            this.Raise(WebExceptionReceived, twitterException);

            // Cannot throw from an interface :(
            return twitterException as TwitterException;
        }
    }
}