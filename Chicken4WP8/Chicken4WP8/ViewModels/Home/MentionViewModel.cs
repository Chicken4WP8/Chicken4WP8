using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Models.Tombstoning;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;
using Chicken4WP8.ViewModels.Status;

namespace Chicken4WP8.ViewModels.Home
{
    public class MentionViewModel : TweetPivotItemViewModelBase
    {
        #region properties
        protected ITweetController statusController;
        protected IUserController userController;

        public MentionViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<ITweetController, OAuthTypeMetadata>> statusControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            statusController = statusControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }
        #endregion

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["MentionViewModel_Header"];
        }

        protected override Task RealizeItem(ITweetModel item)
        {
            var user = item.RetweetedStatus == null ? item.User : item.RetweetedStatus.User;
            if (user.ProfileImageData == null)
                Task.Factory.StartNew(() => userController.SetProfileImageAsync(user));
            return Task.Delay(0);
        }

        protected override async Task InitLoadDataFromWeb()
        {
            var tombstone = StorageService.GetTombstoningData<MentionViewTombstoningData>(TombstoningType.MentionView, App.UserSetting.Id.ToString());
            if (tombstone != null)
            {
                if (tombstone.Mentions != null && tombstone.Mentions.Count != 0)
                    foreach (var item in tombstone.Mentions)
                        Items.Add(item);
                if (tombstone.FetchedItemsCache != null)
                    fetchedItemsCache.AddRange(tombstone.FetchedItemsCache);
                if (tombstone.LoadedItemsCache != null)
                    loadedItemsCache.AddRange(tombstone.LoadedItemsCache);
                if (tombstone.MissedItemsCache != null)
                    missedItemsCache.AddRange(tombstone.MissedItemsCache);
            }
            else
                await base.InitLoadDataFromWeb();
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options)
        {
            var tweets = await statusController.MentionsTimelineAsync(options);
            if (tweets != null)
                return tweets.ToList();
            return null;
        }

        protected override void OnDeactivate(bool close)
        {
            var mentions = Items.ToList();
            var tombstone = new MentionViewTombstoningData
            {
                Mentions = mentions,
                FetchedItemsCache = fetchedItemsCache,
                LoadedItemsCache = loadedItemsCache,
                MissedItemsCache = missedItemsCache
            };
            if (mentions.Count >= 200)
            {
                mentions = mentions.Take(200).ToList();
                tombstone.LoadedItemsCache.Clear();
            }
            StorageService.AddOrUpdateTombstoningData(TombstoningType.MentionView, App.UserSetting.Id.ToString(), tombstone);
            base.OnDeactivate(close);
        }

        public void AppBar_NewTweet()
        {
            var status = new NewTweetModel();
            StorageService.UpdateTempNewTweet(status);
            NavigationService.UriFor<NewStatusPageViewModel>()
                 .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .Navigate();
        }
    }
}
