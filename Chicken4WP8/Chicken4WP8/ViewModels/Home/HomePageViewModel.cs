using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.ViewModels.Base;

namespace Chicken4WP8.ViewModels.Home
{
    public class HomePageViewModel : PageViewModelBase
    {
        public IndexViewModel IndexViewModel { get; set; }
        public MentionViewModel MentionViewModel { get; set; }

        public HomePageViewModel()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(IndexViewModel);
            Items.Add(MentionViewModel);

            ActivateItem(IndexViewModel);

            AppBarConductor.Mixin(this);
        }
    }
}
