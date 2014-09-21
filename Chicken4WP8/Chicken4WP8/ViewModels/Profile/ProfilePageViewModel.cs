using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfilePageViewModel : PageViewModelBase, IHandle<ProfilePageNavigationArgs>
    {
        #region properties
        protected IUserController userController;
        public IStorageService StorageService { get; set; }
        public IProgressService ProgressService { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public ProfileDetailViewModel ProfileDetailViewModel { get; set; }

        public ProfilePageViewModel(
            IEventAggregator eventAggregator,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
        {
            eventAggregator.Subscribe(this);
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }

        private string screenName;
        public string ScreenName
        {
            get { return screenName; }
            set
            {
                screenName = "@" + value;
                NotifyOfPropertyChange(() => ScreenName);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(ProfileDetailViewModel);

            ActivateItem(ProfileDetailViewModel);

            AppBarConductor.Mixin(this);

            Task.Factory.StartNew(() => SetProfileDetail());
        }
        #endregion

        public void Handle(ProfilePageNavigationArgs message)
        {
            ActivateItem(ProfileDetailViewModel);
        }

        private async void SetProfileDetail()
        {
            await ProgressService.ShowAsync();
            //initialize the user from cache
            var user = StorageService.GetTempUser();
            if (user == null)
            {
                var option = Const.GetDictionary();
                option.Add(Const.USER_SCREEN_NAME, user.ScreenName);
                option.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
                user = await userController.ShowAsync(option);
                StorageService.AddOrUpdateCachedUser(user);
            }

            user.IsProfileDetail = true;

            foreach (var item in Items)
                (item as IHaveAUser).User = user;

            await ProgressService.HideAsync();
        }
    }
}
