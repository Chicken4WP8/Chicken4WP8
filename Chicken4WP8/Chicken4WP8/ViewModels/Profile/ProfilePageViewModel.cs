using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Profile
{
    public class ProfilePageViewModel : PageViewModelBase
    {
        public ProfileDetailViewModel ProfileDetailViewModel { get; set; }

        public ProfilePageViewModel()
        { }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(ProfileDetailViewModel);

            ActivateItem(ProfileDetailViewModel);

            AppBarConductor.Mixin(this);
        }
    }
}
