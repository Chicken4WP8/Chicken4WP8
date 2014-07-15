using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;

namespace Chicken4WP8.ViewModels.Home
{
    public class HomePageViewModel : Conductor<Screen>.Collection.OneActive
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
