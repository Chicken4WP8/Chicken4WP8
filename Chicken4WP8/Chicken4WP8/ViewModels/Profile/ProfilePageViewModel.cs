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
        public string DisplayName
        {
            get { return screenName; }
            set
            {
                screenName = "@" + value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(ProfileDetailViewModel);

            ActivateItem(ProfileDetailViewModel);

            AppBarConductor.Mixin(this);

            //initialize the user from cache
            var args = StorageService.GetTempUser();
            Task.Factory.StartNew(() => SetProfileDetail(args));
        }
        #endregion

        public void Handle(ProfilePageNavigationArgs message)
        {
            ActivateItem(ProfileDetailViewModel);
        }

        private async void SetProfileDetail(ProfilePageNavigationArgs args)
        {
            await ProgressService.ShowAsync();
            if (args.User == null)
            {
                var option = Const.GetDictionary();
                if (args.Mention.Id != 0)
                    option.Add(Const.USER_ID, args.Mention.Id);
                if (!string.IsNullOrEmpty(args.Mention.ScreenName))
                    option.Add(Const.USER_SCREEN_NAME, args.Mention.ScreenName);
                option.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
                args.User = await userController.ShowAsync(option);
            }
            args.User.IsProfileDetail = true;
            EventAggregator.Publish(args.User, action => Task.Factory.StartNew(action));
            await ProgressService.HideAsync();
        }
    }
}
