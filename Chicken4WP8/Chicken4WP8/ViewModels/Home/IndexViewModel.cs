using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Implementation;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class IndexViewModel : TweetPivotItemViewModelBase
    {
        #region properties
        protected IStatusController statusController;
        protected IUserController userController;

        public IndexViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> statusControllers,
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

        protected override async Task RealizeItem(ITweetModel item)
        {
            var user = item.RetweetedStatus == null ? item.User : item.RetweetedStatus.User;
            if (user.ProfileImageData != null)
                return;
            Task.Factory.StartNew(() => userController.SetProfileImageAsync(user));
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options)
        {
            var tweets = await statusController.HomeTimelineAsync(options);
            if (tweets != null)
                return tweets.ToList();
            return null;
        }
    }
}
