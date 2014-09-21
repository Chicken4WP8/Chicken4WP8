using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Models.Tombstoning;
using Chicken4WP8.Services.Implementation;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;
using Chicken4WP8.ViewModels.Status;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : TweetPivotItemViewModelBase
    {
        #region properties
        protected ITweetController statusController;
        protected IUserController userController;

        public IndexViewModel(
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

        public void AppBar_Next()
        {
            LanguageHelper.SetLanguage(new CultureInfo("zh-CN"));
        }

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["IndexViewModel_Header"];
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
            var tombstone = StorageService.GetTombstoningData<IndexViewTombstoningData>(TombstoningType.IndexView, App.UserSetting.Id.ToString());
            if (tombstone != null && tombstone.Tweets != null && tombstone.Tweets.Count != 0)
                foreach (var item in tombstone.Tweets)
                    Items.Add(item);
            else
                await base.InitLoadDataFromWeb();
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options)
        {
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets != null)
                return tweets.ToList();
            return null;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            var tweets = Items.ToList();
            var tombstone = new IndexViewTombstoningData { Tweets = tweets };
            StorageService.AddOrUpdateTombstoningData(TombstoningType.IndexView, App.UserSetting.Id.ToString(), tombstone);
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
