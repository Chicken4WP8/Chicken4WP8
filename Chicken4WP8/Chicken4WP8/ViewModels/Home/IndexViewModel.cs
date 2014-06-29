using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : PivotItemViewModelBase
    {
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

        public async void AvatarLoaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            var tweet = image.DataContext as ITweet;
            var stream = await tweet.Creator.GetProfileImageStreamAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            Deployment.Current.Dispatcher.BeginInvoke(() => image.Source = bitmapImage);
        }
    }
}
