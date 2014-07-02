using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo> where T : class
    {
        #region properties
        private const double OFFSET = 20;
        private const int ITEMSPERPAGE = 10;
        /// <summary>
        /// the viewport control of longlistselector
        /// </summary>
        private ViewportControl container;
        private List<T> realizedFetchedItems = new List<T>();
        private List<T> realizedLoadedItems = new List<T>();

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
            await FetchMoreData();
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var control = view as FrameworkElement;
            container = control.GetFirstLogicalChildByType<ViewportControl>(true);
            if (container != null)
                container.ManipulationStateChanged += ContainerManipulationStateChanged;
        }
        #endregion

        #region realize item
        public async virtual void ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            Debug.WriteLine("realize an item");
            await RealizeItem(e.Container.Content as T);
        }

        protected async virtual Task RealizeItem(T item)
        {
            return;
        }
        #endregion

        #region fetch data when at top, load data when at bottom
        private async void ContainerManipulationStateChanged(object sender, ManipulationStateChangedEventArgs e)
        {
            if (!IsLoading && container.ManipulationState == ManipulationState.Animating)
            {
                if (IsAtTop())
                {
                    Debug.WriteLine("now at TOP");
                    await FetchMoreData();
                }
                else if (IsAtBottom())
                {
                    Debug.WriteLine("now at BOTTOM");
                    await LoadMoreData();
                }
            }
        }

        private bool IsAtTop()
        {
            if (Math.Abs(container.Viewport.Top - container.Bounds.Top) <= OFFSET)
                return true;
            return false;
        }

        private bool IsAtBottom()
        {
            if (Math.Abs(container.Viewport.Bottom - container.Bounds.Bottom) <= OFFSET)
                return true;
            return false;
        }
        #endregion

        #region set language
        public virtual void Handle(CultureInfo message)
        {
            SetLanguage();
        }

        /// <summary>
        /// set local strings using language helper on start up
        /// </summary>
        protected virtual void SetLanguage()
        { }
        #endregion

        #region Item click
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
        #endregion

        #region progress bar
        protected async virtual Task ShowProgressBar()
        {
            IsLoading = true;
            await ProgressService.ShowAsync();
        }

        protected async virtual Task HideProgressBar()
        {
            IsLoading = false;
            await ProgressService.HideAsync();
        }
        #endregion

        #region fetch data
        private async Task FetchMoreData()
        {
            await ShowProgressBar();
            int count = realizedFetchedItems.Count;
            Debug.WriteLine("realizedFetchedItems' count is: {0}", count);
            #region add items from cache, with 10 items per action
            if (count > 0)
            {
                if (count > ITEMSPERPAGE)
                {
                    for (int i = 0; i < ITEMSPERPAGE; i++)
                        Items.Add(realizedFetchedItems[count - 1 - i]);
                    realizedFetchedItems.RemoveRange(count - 1 - ITEMSPERPAGE, ITEMSPERPAGE);
                }
                else
                {
                    for (int i = count - 1; i >= 0; i--)
                        Items.Add(realizedFetchedItems[i]);
                    realizedFetchedItems.Clear();
                }
            }
            #endregion
            #region fetch data from derived class
            else
            {
                Debug.WriteLine("fetch data from internet");
                var fetchedList = await FetchData();
                Debug.WriteLine("fetced data count is :{0}", fetchedList.Count());
                realizedFetchedItems.AddRange(fetchedList);
                await FetchMoreData();
            }
            #endregion
            await HideProgressBar();
        }

        protected async virtual Task<IEnumerable<T>> FetchData()
        {
            return new List<T>();
        }
        #endregion

        #region load data
        protected async virtual Task LoadMoreData()
        { }
        #endregion

        #region set image stream
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
        #endregion
    }

    public enum LongLiseStretch
    {
        None,
        Top,
        Bottom,
    }
}
