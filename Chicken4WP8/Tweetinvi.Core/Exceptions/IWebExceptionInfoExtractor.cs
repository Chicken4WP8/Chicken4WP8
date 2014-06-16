using System.Collections.Generic;
using System.Net;

namespace Tweetinvi.Core.Exceptions
{
    public interface IWebExceptionInfoExtractor
    {
        int GetWebExceptionStatusNumber(WebException wex);
        string GetStatusCodeDescription(int statusCode);
        IEnumerable<ITwitterExceptionInfo> GetTwitterExceptionInfo(WebException wex);
    }
}