using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo>
        where T : class
    {
        private const int OFFSET = 1;

        public IProgressService ProgressService { get; set; }
        public ILanguageHelper LanguageHelper { get; set; }
        public INavigationService NavigationService { get; set; }

        protected PivotItemViewModelBase()
        {
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();

            SetLanguage();
        }

        private bool isLoading;
        /// <summary>
        /// if the list is loading data from internet or not
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
            }
        }

        private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<T>();
            //when initialize a pivot item,
            //load data first.
            await ShowProgressBar();
            await FetchData();
            await HideProgressBar();
        }

        public virtual void Handle(CultureInfo message)
        {
            SetLanguage();
        }

        public virtual void AvatarClick(object sender, RoutedEventArgs e)
        {
            //var tweet = sender as Tweet;
            //if (tweet.RetweetStatus != null)
            //    storageService.UpdateTempUser(tweet.RetweetStatus.User);
            //else
            //    storageService.UpdateTempUser(tweet.User);
            //NavigationService.UriFor<ProfilePageViewModel>()
            //    .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
            //    .Navigate();
        }

        public virtual void ItemClick(object sender, RoutedEventArgs e)
        {
            //var tweet = sender as Tweet;
            //storageService.UpdateTempTweet(tweet);
            //navigationService.UriFor<StatusPageViewModel>()
            //    .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
            //    .Navigate();
        }

        public async virtual void ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var item = e.Container.Content as T;
            await ItemRealized(item);
            #region load or fetch data
            //if (!IsLoading && Items.Count >= OFFSET && e.ItemKind == LongListSelectorItemKind.Item)
            //{
            //    //stretch to bottom,
            //    //then load data
            //    if (item.Equals(Items[Items.Count - OFFSET]))
            //    {
            //        await ShowProgressBar();
            //        await LoadData();
            //        await HideProgressBar();
            //    }
            //    //stretch to top,
            //    //then fetch data
            //    else if (item.Equals(Items[0]))
            //    {
            //        await ShowProgressBar();
            //        await FetchData();
            //        await HideProgressBar();
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// set local strings using language helper on start up
        /// </summary>
        protected virtual void SetLanguage()
        { }

        protected async virtual Task ItemRealized(T item)
        { }

        protected async virtual Task ShowProgressBar()
        {
            IsLoading = true;
            await ProgressService.ShowAsync();
        }

        protected async virtual Task FetchData()
        { }

        protected async virtual Task LoadData()
        { }

        protected async virtual Task HideProgressBar()
        {
            IsLoading = false;
            await ProgressService.HideAsync();
        }

        protected virtual void SetImageFromStream(IImageSource source, Stream stream)
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
                            source.ImageSource = bitmapImage;
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
                                source.ImageSource = ei.ToBitmap();
                            };
                        }
                    }
                    #endregion
                });
        }
    }

    public enum LongLiseStretch
    {
        None,
        Top,
        Bottom,
    }
}
