using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chicken4WP8.Controls;
using Chicken4WP8.ViewModels.Base;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;
using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase
    {
        static IndexViewModel()
        {
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }

        protected override void OnPivotItemInitialize()
        {
            base.OnPivotItemInitialize();
            if (Items == null)
                Items = new ObservableCollection<ITweet>();

            RefreshData();
        }

        private ObservableCollection<ITweet> items;
        public ObservableCollection<ITweet> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        protected override async void RefreshData()
        {
            var loggedUser = App.LoggedUser;
            var tweets = await loggedUser.GetHomeTimelineAsync();

            foreach (var tweet in tweets)
                Items.Add(tweet);
            HideProgressBar();
        }

        public void AvatarLoaded(object sender)
        {
            var image = sender as Image;

            //image.SourceUrlChanged -= image_SourceUrlChanged;
            //image.SourceUrlChanged += image_SourceUrlChanged;
        }

        private async void image_SourceUrlChanged(object sender)
        {
            var image = sender as Image;
            var tweet = image.DataContext as ITweet;
            var stream = await tweet.Creator.GetProfileImageStreamAsync();
            ApplyImageSource(image, stream);
        }

        private void ApplyImageSource(Image image, System.IO.Stream stream)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    #region jpeg/png
                    try
                    {
                        using (var memStream = new MemoryStream())
                        {
                            stream.CopyTo(memStream);
                            memStream.Position = 0;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(memStream);
                            //image.ApplySource(bitmapImage);
                        }
                    }
                    #endregion
                    #region others
                    catch (Exception exception)
                    {
                        Debug.WriteLine("set gif image. length: {0}", stream.Length);
                        using (var memStream = new MemoryStream())
                        {
                            stream.CopyTo(memStream);
                            memStream.Position = 0;
                            memStream.Position = 0;
                            var extendedImage = new ExtendedImage();
                            extendedImage.SetSource(memStream);
                            extendedImage.LoadingCompleted += (o, e) =>
                            {
                                var ei = o as ExtendedImage;
                                //image.ApplySource(ei.ToBitmap());
                            };
                        }
                    }
                    #endregion
                });
        }
    }
}
