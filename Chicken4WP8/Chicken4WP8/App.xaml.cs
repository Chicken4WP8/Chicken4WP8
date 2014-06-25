using System.Windows;
using Chicken4WP8.Models.Setting;

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

        public static void UpdateSetting(UserSetting setting)
        {
            _setting = setting;
        }
    }
}