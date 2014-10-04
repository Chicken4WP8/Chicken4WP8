using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controls;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Profile;
using Chicken4WP8.ViewModels.Status;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase : Screen
    {
        public int Index { get; set; }
    }

    public abstract class PivotItemViewModelBase<T> : PivotItemViewModelBase, IHandle<CultureInfo> where T : class
    {
        #region properties
        protected const int ITEMSPERPAGE = 10;
        private const double OFFSET = Const.DEFAULTCOUNT;
        private double height, maxHeight;
        protected LongListSelector listbox;
        private ViewportControl container;
        private Rect oldViewport = Rect.Empty;
        private FrameworkElement footer;
        private List<T> realizedItems = new List<T>();

        private IEventAggregator eventAggregator;
        public ILanguageHelper LanguageHelper { get; set; }
        public IProgressService ProgressService { get; set; }
        public INavigationService NavigationService { get; set; }
        public IStorageService StorageService { get; set; }

        protected PivotItemViewModelBase(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper)
        {
            eventAggregator.Subscribe(this);
            this.eventAggregator = eventAggregator;
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

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var control = view as FrameworkElement;
            listbox = control.GetFirstLogicalChildByType<LongListSelector>(true);
            container = listbox.GetFirstLogicalChildByType<ViewportControl>(true);
            footer = listbox.ListFooter as FrameworkElement;
            footer.Height = 0;

            container.ManipulationStateChanged += ManipulationStateChanged;
            container.ViewportChanged += ViewportChanged;
            listbox.Loaded += ListboxLoaded;
            listbox.ItemRealized += ItemRealized;
            listbox.ItemUnrealized += ItemUnrealized;
        }

        private void ViewportChanged(object sender, ViewportChangedEventArgs e)
        {
            if (oldViewport != Rect.Empty)
            {
                if (container.Viewport.Top > oldViewport.Top)
                    eventAggregator.Publish(new HomePageScreenArgs { IsFullScreen = true }, action => Task.Factory.StartNew(action));
                else if (container.Viewport.Top < oldViewport.Top)
                    eventAggregator.Publish(new HomePageScreenArgs { IsFullScreen = false }, action => Task.Factory.StartNew(action));
            }
            oldViewport = container.Viewport;
        }

        #endregion

        #region realize and unrealize an item
        private void ListboxLoaded(object sender, RoutedEventArgs e)
        {
            maxHeight = listbox.ActualHeight;
            footer.Height = maxHeight + OFFSET;
        }

        private async void ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (e.ItemKind == LongListSelectorItemKind.Item)
            {
                var item = e.Container.Content as T;
                if (!realizedItems.Contains(item))
                {
                    var grid = e.Container.GetFirstLogicalChildByType<Grid>(true);
                    realizedItems.Add(item);
                    grid.SizeChanged += ItemSizeChanged;
                }

                await RealizeItem(item);
            }
        }

        private void ItemSizeChanged(object sender, SizeChangedEventArgs e)
        {
            height += e.NewSize.Height - e.PreviousSize.Height;
            if (height <= maxHeight)
                footer.Height = maxHeight + OFFSET - height;
            else
                footer.Height = 0;
        }

        protected virtual Task RealizeItem(T item)
        {
            return Task.Delay(0);
        }

        private async void ItemUnrealized(object sender, ItemRealizationEventArgs e)
        {
            if (e.ItemKind == LongListSelectorItemKind.Item)
            {
                await UnrealizeItem(e.Container.Content as T);
            }
        }

        protected virtual Task UnrealizeItem(T item)
        {
            return Task.Delay(0);
        }
        #endregion

        #region fetch data when at top, load data when at bottom
        private async void ManipulationStateChanged(object sender, ManipulationStateChangedEventArgs e)
        {
            if (!IsLoading && container.ManipulationState == ManipulationState.Animating)
            {
                if (IsAtTop())
                {
                    Debug.WriteLine("now at TOP");
                    await FetchDataFromWeb();
                }
                else if (IsAtBottom())
                {
                    Debug.WriteLine("now at BOTTOM");
                    await LoadDataFromWeb();
                }
            }
        }

        #region fetch data
        protected async virtual Task FetchDataFromWeb()
        {
            await ShowProgressBar();
            await FetchMoreDataFromWeb();
            await HideProgressBar();
        }

        protected virtual Task FetchMoreDataFromWeb()
        {
            return Task.Delay(0);
        }
        #endregion

        #region load data
        protected async virtual Task LoadDataFromWeb()
        {
            await ShowProgressBar();
            await LoadMoreDataFromWeb();
            await HideProgressBar();
        }

        protected virtual Task LoadMoreDataFromWeb()
        {
            return Task.Delay(0);
        }
        #endregion

        private bool IsAtTop()
        {
            if ((container.Viewport.Top <= container.Bounds.Top))
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
        public void Handle(CultureInfo message)
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
        public void AvatarClick(object sender, RoutedEventArgs e)
        {
            AvatarClicked(sender);
        }

        protected virtual void AvatarClicked(object item)
        {
            var tweet = item as ITweetModel;
            var user = tweet.RetweetedStatus == null ? tweet.User : tweet.RetweetedStatus.User;
            StorageService.UpdateTempUserName(user.ScreenName);
            NavigationService.UriFor<ProfilePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .WithParam(o => o.ScreenName, user.ScreenName)
                .Navigate();
        }

        public void ItemClick(object sender, RoutedEventArgs e)
        {
            ItemClicked(sender);
        }

        protected virtual void ItemClicked(object item)
        {
            var tweet = item as ITweetModel;
            StorageService.UpdateTempTweetId(tweet.Id);
            NavigationService.UriFor<StatusPageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .Navigate();
        }

        public void AutoRichTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as AutoRichTextBox;
            box.HyperlinkClick += (obj, args) =>
            {
                var hyperlink = obj as Hyperlink;
                var entity = hyperlink.CommandParameter as IEntity;
                EntityClicked(entity);
            };
        }

        protected virtual void EntityClicked(IEntity entity)
        {
            switch (entity.EntityType)
            {
                case EntityType.UserMention:
                    var mention = entity as IUserMentionEntity;
                    StorageService.UpdateTempUserName(mention.ScreenName);
                    NavigationService.UriFor<ProfilePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .WithParam(o => o.ScreenName, mention.ScreenName)
                .Navigate();
                    break;
                default:
                    break;
            }
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
    }
}
