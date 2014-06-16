using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    public interface ITwitterRequestGenerator
    {
        /// <summary>
        /// Get the HttpWebRequest expected from the given parameters
        /// </summary>
        HttpWebRequest GetQueryWebRequest(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null);

        HttpWebRequest GetQueryWebRequestWithTemporaryCredentials(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> parameters = null);

        IMultipartWebRequest ExecuteMediaQueryWebRequest(string url,  HttpMethod httpMethod, IEnumerable<IMedia> medias);
    }
}
