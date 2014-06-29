using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase<T> : Screen, IHandle<CultureInfo>
    {
        private LongLiseStretch stretch;
        public IProgressService ProgressService { get; set; }
        public ILanguageHelper LanguageHelper { get; set; }
        public INavigationService NavigationService { get; set; }

        protected PivotItemViewModelBase()
        {
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
            await RefreshData();
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

        //public void StretchingCompleted(object sender, EventArgs e)
        //{
        //    if (IsLoading)
        //        return;

        //    ShowProgressBar();

        //    switch (stretch)
        //    {
        //        case LongLiseStretch.Top:
        //            RefreshData();
        //            break;
        //        case LongLiseStretch.Bottom:
        //            LoadData();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //public void StretchingBottom(object sender, EventArgs e)
        //{
        //    stretch = LongLiseStretch.Bottom;
        //}

        //public void StretchingTop(object sender, EventArgs e)
        //{
        //    stretch = LongLiseStretch.Top;
        //}

        /// <summary>
        /// set local strings using language helper on start up
        /// </summary>
        protected virtual void SetLanguage()
        { }

        protected async virtual Task ShowProgressBar()
        {
            IsLoading = true;
            await ProgressService.ShowAsync();
        }

        protected async virtual Task RefreshData()
        { }

        protected async virtual Task LoadData()
        { }

        protected async virtual Task HideProgressBar()
        {
            IsLoading = false;
            await ProgressService.HideAsync();
        }
    }

    public enum LongLiseStretch
    {
        None,
        Top,
        Bottom,
    }
}
