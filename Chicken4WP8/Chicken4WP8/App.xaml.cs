using System.Windows;
using Chicken4WP8.Models.Setting;
using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private static UserSetting _setting;
        public static UserSetting UserSetting
        {
            get { return _setting; }
        }

        private static ILoggedUser _loggedUser;
        public static ILoggedUser LoggedUser
        {
            get { return _loggedUser; }
        }

        public static void UpdateSetting(UserSetting setting)
        {
            _setting = setting;
        }

        public static void UpdateLoggedUser(ILoggedUser user)
        {
            _loggedUser = user;
        }
    }
}