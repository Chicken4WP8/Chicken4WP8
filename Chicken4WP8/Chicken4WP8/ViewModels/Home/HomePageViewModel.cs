using System.Windows;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.ViewModels.Base;
using Chicken4WP8.Views.Home;

namespace Chicken4WP8.ViewModels.Home
{
    public class HomePageViewModel : PageViewModelBase, IHandle<HomePageScreenArgs>
    {
        #region properties
        private const string NormalScreenState = "NormalScreen";
        private const string FullScreenState = "FullScreen";
        private HomePageView homeView;

        public IndexViewModel IndexViewModel { get; set; }
        public MentionViewModel MentionViewModel { get; set; }
        public DirectMessageViewModel DirectMessageViewModel { get; set; }

        private string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        public HomePageViewModel(IEventAggregator eventAggregator)
            : base()
        {
            eventAggregator.Subscribe(this);
        }
        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(IndexViewModel);
            Items.Add(MentionViewModel);
            Items.Add(DirectMessageViewModel);

            ActivateItem(IndexViewModel);

            AppBarConductor.Mixin(this);
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            homeView = view as HomePageView;
        }

        public void Handle(HomePageScreenArgs message)
        {
            if (message.IsFullScreen && State != FullScreenState)
                EnterFullScreen();
            else if (message.IsFullScreen == false && State == FullScreenState)
                ExitFullScreen();
        }

        private void ExitFullScreen()
        {
            State = NormalScreenState;
            //homeView.Items.Margin = new Thickness(0, 0, 0, -120);
        }

        private void EnterFullScreen()
        {
            State = FullScreenState;
            //homeView.Items.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}
