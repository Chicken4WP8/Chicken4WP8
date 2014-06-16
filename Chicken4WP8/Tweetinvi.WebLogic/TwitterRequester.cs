using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequester : ITwitterRequester
    {
        private readonly ITwitterRequestGenerator _twitterRequestGenerator;
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequester(ITwitterRequestGenerator twitterRequestGenerator, IWebRequestExecutor webRequestExecutor)
        {
            _twitterRequestGenerator = twitterRequestGenerator;
            _webRequestExecutor = webRequestExecutor;
        }

        public string ExecuteQuery(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequest(url, httpMethod, headers);
            return _webRequestExecutor.ExecuteWebRequest(webRequest);
        }

        public string ExecuteMultipartQuery(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias)
        {
            if (medias == null || medias.IsEmpty())
            {
                return ExecuteQuery(url, httpMethod);
            }

            var webRequest = _twitterRequestGenerator.ExecuteMediaQueryWebRequest(url, httpMethod, medias);
            return _webRequestExecutor.ExecuteMultipartRequest(webRequest);
        }

        public string ExecuteQueryWithTemporaryCredentials(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> parameters)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequestWithTemporaryCredentials(url, httpMethod, temporaryCredentials, parameters);
            return _webRequestExecutor.ExecuteWebRequest(webRequest);
        }

    }
}