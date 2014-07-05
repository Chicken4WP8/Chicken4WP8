using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Chicken4WP8.Models.Setting;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public abstract class BaseControllerBase
    {
        protected Tokens tokens;

        public BaseControllerBase()
        {
            var oauth = App.UserSetting.OAuthSetting as BaseOAuthSetting;
            tokens = Tokens.Create(oauth.ConsumerKey, oauth.ConsumerSecret, oauth.AccessToken, oauth.AccessTokenSecret);
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
                    var stream = rt.Result.GetResponseStream();
                     int count = (int)stream.Length;
                     var data = new byte[count];
                     stream.Write(data, 0, count);
                     return data;
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
                    //Debug.WriteLine("set gif image. length: {0}", stream.Length);
                    //using (var memStream = new MemoryStream())
                    //{
                    //    stream.CopyTo(memStream);
                    //    memStream.Position = 0;
                    //    memStream.Position = 0;
                    //    var extendedImage = new ExtendedImage();
                    //    extendedImage.SetSource(memStream);
                    //    extendedImage.LoadingCompleted += (o, e) =>
                    //    {
                    //        var ei = o as ExtendedImage;
                    //        source.ImageSource = ei.ToBitmap();
                    //    };
                    //}
                }
                #endregion
            });
        }
        #endregion
    }
}
