using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfilePageViewModel : PageViewModelBase, IHandle<ProfilePageNavigationArgs>
    {
        #region properties
        public IEventAggregator EventAggregator { get; set; }
        public ProfileDetailViewModel ProfileDetailViewModel { get; set; }

        public ProfilePageViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        #endregion
        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(ProfileDetailViewModel);

            ActivateItem(ProfileDetailViewModel);

            AppBarConductor.Mixin(this);
        }

        public void Handle(ProfilePageNavigationArgs message)
        {
            ActivateItem(ProfileDetailViewModel);
        }
    }
}
