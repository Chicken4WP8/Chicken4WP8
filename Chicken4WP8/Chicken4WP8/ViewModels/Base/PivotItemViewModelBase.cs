using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo> where T : class
    {
        #region properties
        private const double OFFSET = 20;
        protected const int ITEMSPERPAGE = 10;
        /// <summary>
        /// the viewport control of longlistselector
        /// </summary>
        private ViewportControl container;

        public ILanguageHelper LanguageHelper { get; set; }
        public IProgressService ProgressService { get; set; }
        public INavigationService NavigationService { get; set; }
        public IStorageService StorageService { get; set; }

        protected PivotItemViewModelBase(ILanguageHelper languageHelper)
        {
            LanguageHelper = languageHelper;
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SetLanguage();
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

        #region realize and unrealize an item
        public async virtual void ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            //Debug.WriteLine("realize an item");
            await RealizeItem(e.Container.Content as T);
        }

        protected async virtual Task RealizeItem(T item)
        {
            return;
        }

        public async virtual void ItemUnrealized(object sender, ItemRealizationEventArgs e)
        {
            //Debug.WriteLine("unrealize an item");
            await UnrealizeItem(e.Container.Content as T);
        }

        protected async virtual Task UnrealizeItem(T item)
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
                    await ShowProgressBar();
                    await FetchMoreDataFromWeb();
                    await HideProgressBar();

                }
                else if (IsAtBottom())
                {
                    Debug.WriteLine("now at BOTTOM");
                    await ShowProgressBar();
                    await LoadMoreDataFromWeb();
                    await HideProgressBar();
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
        protected abstract Task FetchMoreDataFromWeb();
        #endregion

        #region load data
        protected abstract Task LoadMoreDataFromWeb();
        #endregion
    }

    public enum LongLiseStretch
    {
        None,
        Top,
        Bottom,
    }
}
