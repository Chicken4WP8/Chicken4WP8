using Caliburn.Micro;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Setting.Proxies
{
    public class CustomerKeyAndSecretPageViewModel : Screen
    {
        public INavigationService NavigationService { get; set; }
        public ILanguageHelper LanguageHelper { get; set; }

        private string key = "yN3DUNVO0Me63IAQdhTfCA";
        public string CustomerKey
        {
            get { return key; }
            set
            {
                key = value;
                NotifyOfPropertyChange(() => CustomerKey);
            }
        }

        private string secret = "c768oTKdzAjIYCmpSNIdZbGaG0t6rOhSFQP0S5uC79g";
        public string CustomerSecret
        {
            get { return secret; }
            set
            {
                secret = value;
                NotifyOfPropertyChange(() => CustomerSecret);
            }
        }

        public void AppBar_Next()
        {
            if (string.IsNullOrEmpty(CustomerKey) || string.IsNullOrEmpty(CustomerSecret))
                return;
            NavigationService.UriFor<CustomerOAuthSettingPageViewModel>()
                .WithParam(m => m.Key, CustomerKey)
                .WithParam(m => m.Secret, CustomerSecret)
                .Navigate();
        }
    }
}
