using System.Collections.ObjectModel;
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
            var tweets =await loggedUser.GetHomeTimelineAsync();

            foreach (var tweet in tweets)
                Items.Add(tweet);
            HideProgressBar();
        }
    }
}
