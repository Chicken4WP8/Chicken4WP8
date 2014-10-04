using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Chicken4WP8.Models.Setting;
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
        private List<PivotItemViewModelBase> models;

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

        public HomePageViewModel(
            IEventAggregator eventAggregator,
            IEnumerable<Lazy<PivotItemViewModelBase, HomePageSettingTypeMetadata>> viewModels)
            : base()
        {
            eventAggregator.Subscribe(this);
            models = new List<PivotItemViewModelBase>();
            foreach (var setting in App.UserSetting.HomePageSettings.Settings)
            {
                var model = viewModels.Single(m => m.Metadata.Type == setting.Type).Value;
                model.Index = setting.Index;
                model.Title = setting.Title;
                models.Add(model);
            }
            models = models.OrderBy(m => m.Index).ToList();
        }
        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var model in models)
                Items.Add(model);
            ActivateItem(models[0]);

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
