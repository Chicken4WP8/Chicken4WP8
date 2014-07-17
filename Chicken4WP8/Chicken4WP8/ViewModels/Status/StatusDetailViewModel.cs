using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controls;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Status
{
    public class StatusDetailViewModel : PivotItemViewModelBase<ITweetModel>
    {
        public StatusDetailViewModel(ILanguageHelper languageHelper)
            : base(languageHelper)
        { }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<ITweetModel>();

            await ShowProgressBar();
            //initialize the tweet from cache
            var tweet = StorageService.GetTempTweet();
            Items.Add(tweet);
            await HideProgressBar();
        }

        public virtual void AutoRichTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as AutoRichTextBox;
            box.HyperlinkClick += (obj, args) =>
            {

            };
        }

        protected override Task FetchMoreDataFromWeb()
        {
            throw new NotImplementedException();
        }

        protected override Task LoadMoreDataFromWeb()
        {
            throw new NotImplementedException();
        }
    }
}
