using System.Collections.ObjectModel;
using Caliburn.Micro;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Setting.Proxies;

namespace Chicken4WP8.ViewModels.Setting
{
    public class OAuthSettingPageViewModel : Screen
    {
        public ILanguageHelper LanguageHelper { get; set; }
        public IStorageService StorageSerive { get; set; }
        public INavigationService NavigationService { get; set; }

        public OAuthSettingPageViewModel()
        { }

        private ObservableCollection<IOAuthSetting> types;
        public ObservableCollection<IOAuthSetting> Items
        {
            get { return types; }
            set
            {
                types = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        private IOAuthSetting type;
        public IOAuthSetting SelectedItem
        {
            get { return type; }
            set
            {
                type = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var @base = new BaseOAuthSetting();
            var twip = new TwipOAuthSetting();

            Items = new ObservableCollection<IOAuthSetting>();
            Items.Add(@base);
            Items.Add(twip);

            SelectedItem = Items[0];
        }

        public void AppBar_Next()
        {
            switch (SelectedItem.Name)
            {
                case "BASE":
                    break;
                case "TWIP4":
                    NavigationService.UriFor<TwipOAuthSettingPageViewModel>().Navigate();
                    break;
            }
        }
    }
}
