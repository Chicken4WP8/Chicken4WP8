using System.Threading.Tasks;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase<TweetModel>
    {
        protected override async Task RefreshData()
        {
            var loggedUser = App.LoggedUser;
            var tweets = await loggedUser.GetHomeTimelineAsync();

            foreach (var tweet in tweets)
                Items.Add(new TweetModel(tweet));
        }

        //public void AvatarLoaded(object sender)
        //{
        //    var image = sender as Image;

        //    //image.SourceUrlChanged -= image_SourceUrlChanged;
        //    //image.SourceUrlChanged += image_SourceUrlChanged;
        //}

        //private async void image_SourceUrlChanged(object sender)
        //{
        //    var image = sender as Image;
        //    var tweet = image.DataContext as ITweet;
        //    var stream = await tweet.Creator.GetProfileImageStreamAsync();
        //    ApplyImageSource(image, stream);
        //}

        //private void ApplyImageSource(Image image, System.IO.Stream stream)
        //{
        //    Deployment.Current.Dispatcher.BeginInvoke(() =>
        //        {
        //            #region jpeg/png
        //            try
        //            {
        //                using (var memStream = new MemoryStream())
        //                {
        //                    stream.CopyTo(memStream);
        //                    memStream.Position = 0;
        //                    var bitmapImage = new BitmapImage();
        //                    bitmapImage.SetSource(memStream);
        //                    //image.ApplySource(bitmapImage);
        //                }
        //            }
        //            #endregion
        //            #region others
        //            catch (Exception exception)
        //            {
        //                Debug.WriteLine("set gif image. length: {0}", stream.Length);
        //                using (var memStream = new MemoryStream())
        //                {
        //                    stream.CopyTo(memStream);
        //                    memStream.Position = 0;
        //                    memStream.Position = 0;
        //                    var extendedImage = new ExtendedImage();
        //                    extendedImage.SetSource(memStream);
        //                    extendedImage.LoadingCompleted += (o, e) =>
        //                    {
        //                        var ei = o as ExtendedImage;
        //                        //image.ApplySource(ei.ToBitmap());
        //                    };
        //                }
        //            }
        //            #endregion
        //        });
        //}
    }
}
