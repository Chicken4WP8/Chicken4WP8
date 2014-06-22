using System.Net;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    public interface IWebRequestExecutorAsync
    {
        Task<string> ExecuteWebRequestAsync(HttpWebRequest httpWebRequest);
        Task<string> ExecuteMultipartRequestAsync(IMultipartWebRequest multipartWebRequest);
    }

    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IWebRequestExecutor : IWebRequestExecutorAsync
    {
        string ExecuteWebRequest(HttpWebRequest httpWebRequest);
        string ExecuteMultipartRequest(IMultipartWebRequest multipartWebRequest);
    }
}