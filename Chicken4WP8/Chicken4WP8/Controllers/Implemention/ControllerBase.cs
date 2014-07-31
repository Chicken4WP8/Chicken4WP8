using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Implemention
{
    public class ControllerBase
    {
        protected async Task<byte[]> DownloadImage(string url)
        {
            WebRequest httpWebRequest = WebRequest.Create(new Uri(url, UriKind.Absolute));
            Task<WebResponse> requestTask =
                Task.Factory.FromAsync<WebResponse>(
                httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse,
                httpWebRequest);
            var result = await requestTask.ContinueWith(
             rt =>
             {
                 using (var stream = rt.Result.GetResponseStream())
                 {
                     var memoryStream = new MemoryStream();
                     stream.CopyTo(memoryStream);
                     var data = memoryStream.ToArray();
                     return data;
                 }
             });
            return result;
        }
    }
}
