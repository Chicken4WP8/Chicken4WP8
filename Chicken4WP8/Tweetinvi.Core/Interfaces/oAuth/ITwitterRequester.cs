using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    public interface ITwitterRequesterAsync
    {
        Task<string> ExecuteQueryAsync(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null);
        Task<string> ExecuteMultipartQueryAsync(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias);
        Task<string> ExecuteQueryWithTemporaryCredentialsAsync(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> headers);
    }

    public interface ITwitterRequester : ITwitterRequesterAsync
    {
        string ExecuteQuery(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null);
        string ExecuteMultipartQuery(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias);
        string ExecuteQueryWithTemporaryCredentials(string url, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> headers);
    }
}