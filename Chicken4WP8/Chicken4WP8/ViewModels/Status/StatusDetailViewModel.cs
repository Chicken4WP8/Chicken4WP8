using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Status
{
    public class StatusDetailViewModel : PivotItemViewModelBase<ITweetModel>
    {
        #region properties
        protected ITweetModel status;
        protected IStatusController statusController;
        protected IUserController userController;

        public StatusDetailViewModel(
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

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<ITweetModel>();

            await ShowProgressBar();
            //initialize the tweet from cache
            status = StorageService.GetTempTweet();
            Items.Add(status);
            await HideProgressBar();
        }

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["StatusDetailViewModel_Header"];
        }

        protected override void ItemClicked(object item)
        {
            var tweet = item as ITweetModel;
            if (tweet.Id == status.Id)
                return;
            base.ItemClicked(item);
        }

        protected async override Task RealizeItem(ITweetModel item)
        {
            var user = item.RetweetedStatus == null ? item.User : item.RetweetedStatus.User;
            if (user.ImageSource != null)
                return;
            string id = user.Id.Value + user.ProfileImageUrl;
            var data = StorageService.GetCachedImage(id);
            if (data == null)
            {
                data = await userController.DownloadProfileImageAsync(user.ProfileImageUrl);
                StorageService.AddOrUpdateImageCache(id, data);
            }
            await userController.SetProfileImageAsync(user, data);
        }

        protected async override Task FetchDataFromWeb()
        {
            var id = Items[0].InReplyToTweetId;
            if (id == null)
                return;
            await ShowProgressBar();
            var option = Const.GetDictionary();
            option.Add(Const.ID, id);
            var tweet = await statusController.ShowAsync(option);
            Items.Insert(0, tweet);
            listbox.ScrollTo(Items[0]);
            await HideProgressBar();
        }

        protected async override Task LoadDataFromWeb()
        {
            return;
        }
    }
}
