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
using Chicken4WP8.ViewModels.Profile;

namespace Chicken4WP8.ViewModels.Home
{
    public class DirectMessageViewModel : PivotItemViewModelBase<IDirectMessageModel>
    {
        #region properties
        protected IDirectMessageController messageController;
        protected IUserController userController;

        public DirectMessageViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IDirectMessageController, OAuthTypeMetadata>> messageControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            messageController = messageControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        protected override void SetLanguage()
        {
            if (string.IsNullOrEmpty(Title))
                DisplayName = LanguageHelper["DirectMessageViewModel_Header"];
        }
        #endregion

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<IDirectMessageModel>();

            await ShowProgressBar();
            //when initialize a pivot item,
            //load data from web first
            await InitLoadDataFromCache();
            await HideProgressBar();
        }

        protected override Task RealizeItem(IDirectMessageModel item)
        {
            if (item.User.ProfileImageData == null)
                Task.Factory.StartNew(() => userController.SetProfileImageAsync(item.User));
            return Task.Delay(0);
        }

        protected override void AvatarClicked(object item)
        {
            var message = item as IDirectMessageModel;
            var user = message.User;
            StorageService.UpdateTempUserName(user.ScreenName);
            StorageService.AddOrUpdateCachedUser(user);
            NavigationService.UriFor<ProfilePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .WithParam(o => o.ScreenName, user.ScreenName)
                .Navigate();
        }

        protected override void ItemClicked(object item)
        {
            var message = item as IDirectMessageModel;
            var user = message.User;
            StorageService.UpdateTempDirectMessageUserName(user.ScreenName);
            NavigationService.UriFor<NewDirectMessagePageViewModel>()
                .WithParam(o => o.Random, DateTime.Now.Ticks.ToString("x"))
                .WithParam(o => o.ScreenName, user.ScreenName)
                .Navigate();
        }

        private async Task InitLoadDataFromCache()
        {
            var list = StorageService.GetGroupedDirectMessages();
            if (list != null && list.Count != 0)
            {
                Items.Clear();
                foreach (var item in list.OrderBy(m => m.Id))
                    Items.Insert(0, item);
                listbox.ScrollTo(Items[0]);
            }
            else
                await FetchMoreDataFromWeb();
        }

        protected override async Task FetchMoreDataFromWeb()
        {
            var msgs = new List<IDirectMessageModel>();
            var options = Const.GetDictionary();
            var sinceId = StorageService.GetSendDirectMessageSinceId();
            if (sinceId != null)
                options.Add(Const.SINCE_ID, sinceId);
            var sendMsgs = await messageController.SentAsync(options);
            if (sendMsgs != null && sendMsgs.Count() != 0)
                msgs.AddRange(sendMsgs);
            options.Clear();
            sinceId = StorageService.GetReceivedDirectMessageSinceId();
            if (sinceId != null)
                options.Add(Const.SINCE_ID, sinceId);
            var receivedMsgs = await messageController.ReceivedAsync(options);
            if (receivedMsgs != null && receivedMsgs.Count() != 0)
                msgs.AddRange(receivedMsgs);
            StorageService.AddCachedDirectMessages(msgs);
            var list = StorageService.GetGroupedDirectMessages();
            if (list != null && list.Count != 0)
            {
                Items.Clear();
                foreach (var item in list.OrderBy(m => m.Id))
                    Items.Insert(0, item);
                listbox.ScrollTo(Items[0]);
            }
        }

        protected override async Task LoadMoreDataFromWeb()
        {
            var msgs = new List<IDirectMessageModel>();
            var options = Const.GetDictionary();
            var maxId = StorageService.GetSendDirectMessageMaxId();
            if (maxId != null)
                options.Add(Const.MAX_ID, maxId);
            var sendMsgs = await messageController.SentAsync(options);
            if (sendMsgs != null && sendMsgs.Count() != 0)
                msgs.AddRange(sendMsgs);
            options.Clear();
            maxId = StorageService.GetReceivedDirectMessageMaxId();
            if (maxId != null)
                options.Add(Const.MAX_ID, maxId);
            var receivedMsgs = await messageController.ReceivedAsync(options);
            if (receivedMsgs != null && receivedMsgs.Count() != 0)
                msgs.AddRange(receivedMsgs);
            StorageService.AddCachedDirectMessages(msgs);
            int skip = Items.Count;
            var list = StorageService.GetGroupedDirectMessages();
            if (list != null && list.Count != 0)
            {
                Items.Clear();
                foreach (var item in list)
                    Items.Add(item);
                listbox.ScrollTo(Items[Items.Count - 1]);
            }
        }
    }
}
