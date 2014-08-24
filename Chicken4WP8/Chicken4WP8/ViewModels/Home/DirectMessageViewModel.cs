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
        #endregion

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<IDirectMessageModel>();

            await ShowProgressBar();
            //when initialize a pivot item,
            //load data from web first
            await FetchMoreDataFromWeb();
            await HideProgressBar();
        }

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["DirectMessageViewModel_Header"];
        }

        protected override async Task FetchMoreDataFromWeb()
        {
            var msgs = new List<IDirectMessageModel>();
            var options = Const.GetDictionary();
            options.Add(Const.COUNT, 20);
            var sinceId = StorageService.GetSendDirectMessageSinceId();
            if (sinceId != null)
                options.Add(Const.SINCE_ID, sinceId);
            var sendMsgs = await messageController.SentAsync(options);
            if (sendMsgs != null && sendMsgs.Count() != 0)
                msgs.AddRange(sendMsgs);
            options.Clear();
            options.Add(Const.COUNT, 20);
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
                foreach (var item in list)
                    Items.Add(item);
                listbox.ScrollTo(Items[0]);
            }
        }
    }
}
