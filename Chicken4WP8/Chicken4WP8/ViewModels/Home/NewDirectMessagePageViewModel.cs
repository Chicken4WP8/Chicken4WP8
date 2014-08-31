using Caliburn.Micro;
using Chicken4WP8.Controllers;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class NewDirectMessagePageViewModel : PivotItemViewModelBase<IDirectMessageModel>
    {
        #region properties
        public string Random { get; set; }

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

        private bool isNew=false;
        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                isNew = value;
                NotifyOfPropertyChange(() => IsNew);
            }
        }

        private bool hasError;
        public bool HasError
        {
            get
            {
                return hasError;
            }
            set
            {
                hasError = value;
                NotifyOfPropertyChange(() => HasError);
            }
        }

        public NewDirectMessagePageViewModel(
             IEventAggregator eventAggregator,
            ILanguageHelper languageHelper)
            : base(eventAggregator, languageHelper)
        { }
        #endregion
    }
}
