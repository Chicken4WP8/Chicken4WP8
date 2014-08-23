using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Controllers.Implementation
{
    public abstract class ControllerBase
    {
        public IStorageService StorageService { get; set; }

        protected async Task<byte[]> GetImageAsync(string id, string url)
        {
            var data = StorageService.GetCachedImage(id);
            if (data == null)
            {
                data = await DownloadImage(url);
                data = StorageService.AddOrUpdateImageCache(id, data);
            }
            return data;
        }

        private async Task<byte[]> DownloadImage(string url)
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
