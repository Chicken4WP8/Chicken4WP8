using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Status
{
    public class StatusPageViewModel : PageViewModelBase
    {
        public StatusDetailViewModel StatusDetailViewModel { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(StatusDetailViewModel);

            ActivateItem(StatusDetailViewModel);

            AppBarConductor.Mixin(this);
        }
    }
}
