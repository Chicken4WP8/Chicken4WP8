using Caliburn.Micro;

namespace Chicken4WP8.ViewModels.Home
{
    public class HomePageViewModel : Conductor<Screen>.Collection.OneActive
    {
        public IndexViewModel IndexViewModel { get; set; }

        public HomePageViewModel()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(IndexViewModel);

            ActivateItem(IndexViewModel);
        }
    }
}
