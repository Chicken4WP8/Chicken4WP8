using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;

namespace Chicken4WP8.Controllers.Implemention
{
    public class ControllerBase
    {
        static ControllerBase()
        {
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }

        protected async Task<byte[]> DownloadImage(Uri uri)
        {
            WebRequest httpWebRequest = WebRequest.Create(uri);
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

        #region set image stream
        protected virtual void SetImageFromBytes(IImageSource source, byte[] data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                #region jpeg/png
                try
                {
                    using (var memStream = new MemoryStream(data))
                    {
                        memStream.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(memStream);
                        source.ImageSource = bitmapImage;
                    }
                }
                #endregion
                #region others
                catch (Exception exception)
                {
                    Debug.WriteLine("set gif image. length: {0}", data.Length);
                    using (var memStream = new MemoryStream(data))
                    {
                        memStream.Position = 0;
                        var extendedImage = new ExtendedImage();
                        extendedImage.SetSource(memStream);
                        extendedImage.LoadingCompleted += (o, e) =>
                        {
                            var ei = o as ExtendedImage;
                            source.ImageSource = ei.ToBitmap();
                        };
                    }
                }
                #endregion
            });
        }
        #endregion
    }
}
