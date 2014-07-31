using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfileDetailViewModel : PivotItemViewModelBase<IUserModel>
    {
        #region properties
        protected IUserModel user;
        protected IUserController userController;

        public ProfileDetailViewModel(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }
        #endregion

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<IUserModel>();

            await ShowProgressBar();
            //initialize the user from cache
            user = StorageService.GetTempUser();
            user.IsProfileDetail = true;
            Items.Add(user);
            await HideProgressBar();
        }

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["ProfileDetailViewModel_Header"];
        }

        protected async override Task RealizeItem(IUserModel item)
        {
            if (user.ImageSource != null)
                return;
            string url = user.ProfileImageUrl.Replace("_bigger", "");
            string id = user.Id.Value + url;
            var data = StorageService.GetCachedImage(id);
            if (data == null)
            {
                data = await userController.DownloadProfileImageAsync(url);
                StorageService.AddOrUpdateImageCache(id, data);
            }
            await userController.SetProfileImageAsync(user, data);
        }
    }
}
