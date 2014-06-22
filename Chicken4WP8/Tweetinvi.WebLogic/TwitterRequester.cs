using System.Collections.Generic;
using System.Threading.Tasks;
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

        #region sync
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

        public string ExecuteQueryWithTemporaryCredentials(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> headers)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequestWithTemporaryCredentials(url, httpMethod, temporaryCredentials, headers);
            return _webRequestExecutor.ExecuteWebRequest(webRequest);
        }
        #endregion

        #region async
        public async Task<string> ExecuteQueryAsync(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequest(url, httpMethod, headers);
            return await _webRequestExecutor.ExecuteWebRequestAsync(webRequest);
        }

        public async Task<string> ExecuteMultipartQueryAsync(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias)
        {
            if (medias == null || medias.IsEmpty())
            {
                return await ExecuteQueryAsync(url, httpMethod);
            }

            var webRequest = _twitterRequestGenerator.ExecuteMediaQueryWebRequest(url, httpMethod, medias);
            return await _webRequestExecutor.ExecuteMultipartRequestAsync(webRequest);
        }

        public async Task<string> ExecuteQueryWithTemporaryCredentialsAsync(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> headers)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequestWithTemporaryCredentials(url, httpMethod, temporaryCredentials, headers);
            return await _webRequestExecutor.ExecuteWebRequestAsync(webRequest);
        }
        #endregion
    }
}