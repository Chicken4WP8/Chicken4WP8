using System;
using System.Globalization;
using System.Windows;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class PivotItemViewModelBase : Screen, IHandle<CultureInfo>
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            //when initialize a pivot item,
            //load data first.
            ShowProgressBar();
            OnPivotItemInitialize();
        }

        protected virtual void OnPivotItemInitialize()
        { }

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

        public void StretchingCompleted(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            ShowProgressBar();

            switch (stretch)
            {
                case LongLiseStretch.Top:
                    RefreshData();
                    break;
                case LongLiseStretch.Bottom:
                    LoadData();
                    break;
                default:
                    break;
            }
        }

        public void StretchingBottom(object sender, EventArgs e)
        {
            stretch = LongLiseStretch.Bottom;
        }

        public void StretchingTop(object sender, EventArgs e)
        {
            stretch = LongLiseStretch.Top;
        }

        /// <summary>
        /// set local strings using language helper on start up
        /// </summary>
        protected virtual void SetLanguage()
        { }

        protected virtual void ShowProgressBar()
        {
            IsLoading = true;
            ProgressService.Show();
        }

        protected virtual void RefreshData()
        { }

        protected virtual void LoadData()
        { }

        protected virtual void HideProgressBar()
        {
            IsLoading = false;
            ProgressService.Hide();
        }
    }

    public enum LongLiseStretch
    {
        None,
        Top,
        Bottom,
    }
}
