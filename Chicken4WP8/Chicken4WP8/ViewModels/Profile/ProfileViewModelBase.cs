using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public interface IHaveAUser
    {
        IUserModel User { get; set; }
    }

    public abstract class ProfileViewModelBase<T> : PivotItemViewModelBase<T>, IHaveAUser where T : class
    {
        #region properties
        protected IUserController userController;
        private IUserModel user;
        public IUserModel User
        {
            get { return user; }
            set
            {
                user = value;
                SetUserProfile();
                NotifyOfPropertyChange(() => User);
            }
        }

        public ProfileViewModelBase(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(eventAggregator, languageHelper)
        {
            userController = userControllers.Single(c => c.Metadata.OAuthType == App.UserSetting.OAuthSetting.OAuthSettingType).Value;
        }
        #endregion

        protected virtual void SetUserProfile()
        { }
    }
}
