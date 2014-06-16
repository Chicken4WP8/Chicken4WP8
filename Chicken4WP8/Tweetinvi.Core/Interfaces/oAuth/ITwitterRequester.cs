using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    public interface ITwitterRequester
    {
        string ExecuteQuery(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null);
        string ExecuteMultipartQuery(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias);
        string ExecuteQueryWithTemporaryCredentials(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> headers);
    }
}