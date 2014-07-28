using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controls;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Profile;
using Chicken4WP8.ViewModels.Status;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo> where T : class
    {
        #region properties
        protected const int ITEMSPERPAGE = 10;
        protected AutoListBox listbox;

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
            listbox = control.GetFirstLogicalChildByType<AutoListBox>(true);
            if (listbox != null)
            {
                listbox.VerticalCompressionTopHandler += VerticalCompressionTopHandler;
                listbox.VerticalCompressionBottomHandler += VerticalCompressionBottomHandler;
                listbox.ItemRealizedEventHandler += ItemRealized;
                listbox.ItemUnRealizedEventHandler += ItemUnRealized;
            }
        }
        #endregion

        #region realize and unrealize an item
        private async void ItemRealized(object sender, RealizedItemEventArgs e)
        {
            //Debug.WriteLine("realize an item");
            await RealizeItem(e.Item as T);
        }

        protected async virtual Task RealizeItem(T item)
        {
            return;
        }

        private async void ItemUnRealized(object sender, RealizedItemEventArgs e)
        {
            //Debug.WriteLine("unrealize an item");
            await UnrealizeItem(e.Item as T);
        }

        protected async virtual Task UnrealizeItem(T item)
        {
            return;
        }
        #endregion

        #region fetch data when at top, load data when at bottom
        private async void VerticalCompressionTopHandler(object sender, EventArgs e)
        {
            if (IsLoading)
                return;
            Debug.WriteLine("now at TOP");
            await ShowProgressBar();
            await FetchMoreDataFromWeb();
            if (Items != null && Items.Count != 0)
                listbox.ScrollIntoView(Items[0]);
            listbox.UpdateLayout();
            await HideProgressBar();
        }

        private async void VerticalCompressionBottomHandler(object sender, EventArgs e)
        {
            if (IsLoading)
                return;
            Debug.WriteLine("now at BOTTOM");
            await ShowProgressBar();
            await LoadMoreDataFromWeb();
            await HideProgressBar();
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
            user.IsProfileDetail = true;
            //var temp = StorageService.GetTempUser();
            //if (temp != null && user.ScreenName == temp.ScreenName)
            //{
            //    EventAggregator.PublishOnBackgroundThread(new ProfilePageNavigationArgs());
            //    return;
            //}
            StorageService.UpdateTempUser(user);
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
            tweet.IsStatusDetail = true;
            if (tweet.RetweetedStatus != null)
                tweet.RetweetedStatus.IsStatusDetail = true;
            StorageService.UpdateTempTweet(tweet);
            NavigationService.UriFor<StatusPageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .Navigate();
        }

        public virtual void AutoRichTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as AutoRichTextBox;
            box.HyperlinkClick += (obj, args) =>
            {
                var hyperlink = obj as Hyperlink;
                var entity = hyperlink.CommandParameter as IEntity;
                switch (entity.EntityType)
                {
                    case EntityType.UserMention:
                        break;
                    default:
                        break;
                }
            };
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
        protected async virtual Task FetchMoreDataFromWeb()
        {
            return;
        }
        #endregion

        #region load data
        protected async virtual Task LoadMoreDataFromWeb()
        {
            return;
        }
        #endregion
    }
}
