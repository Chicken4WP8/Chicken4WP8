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
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controls;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Profile;
using Chicken4WP8.ViewModels.Status;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo> where T : class
    {
        #region properties
        protected const int ITEMSPERPAGE = 10;
        private const double OFFSET = 10;
        private double height, maxHeight;
        protected LongListSelector listbox;
        private ViewportControl container;
        private FrameworkElement footer;
        private List<T> realizedItems = new List<T>();

        public IEventAggregator EventAggregator { get; set; }
        public ILanguageHelper LanguageHelper { get; set; }
        public IProgressService ProgressService { get; set; }
        public INavigationService NavigationService { get; set; }
        public IStorageService StorageService { get; set; }

        protected PivotItemViewModelBase(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper)
        {
            eventAggregator.Subscribe(this);
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
            listbox.Loaded += ListboxLoaded;
            listbox.ItemRealized += ItemRealized;
            listbox.ItemUnrealized += ItemUnrealized;
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

        protected async virtual Task RealizeItem(T item)
        {
            return;
        }

        private async void ItemUnrealized(object sender, ItemRealizationEventArgs e)
        {
            if (e.ItemKind == LongListSelectorItemKind.Item)
            {
                await UnrealizeItem(e.Container.Content as T);
            }
        }

        protected async virtual Task UnrealizeItem(T item)
        {
            return;
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

        protected async virtual Task FetchMoreDataFromWeb()
        {
            return;
        }
        #endregion

        #region load data
        protected async virtual Task LoadDataFromWeb()
        {
            await ShowProgressBar();
            await LoadMoreDataFromWeb();
            await HideProgressBar();
        }

        protected async virtual Task LoadMoreDataFromWeb()
        {
            return;
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
            //var temp = StorageService.GetTempUser();
            //if (temp != null && user.ScreenName == temp.ScreenName)
            //{
            //    EventAggregator.PublishOnBackgroundThread(new ProfilePageNavigationArgs());
            //    return;
            //}
            var args = new ProfilePageNavigationArgs { User = user };
            StorageService.UpdateTempUser(args);
            NavigationService.UriFor<ProfilePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .Navigate();
        }

        public void ItemClick(object sender, RoutedEventArgs e)
        {
            ItemClicked(sender);
        }

        protected virtual void ItemClicked(object item)
        {
            var tweet = item as ITweetModel;
            StorageService.UpdateTempTweet(tweet);
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
                    var args = new ProfilePageNavigationArgs { Mention = entity as IUserMentionEntity };
                    StorageService.UpdateTempUser(args);
                    NavigationService.UriFor<ProfilePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
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
