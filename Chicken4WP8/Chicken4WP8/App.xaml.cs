using System.Windows;
using Chicken4WP8.Models.Setting;
using CoreTweet.Core;

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

        private static TokensBase _tokens;
        public static TokensBase Tokens
        {
            get { return _tokens; }
        }

        public static void UpdateSetting(UserSetting setting)
        {
            _setting = setting;
        }

        public static void UpdateTokens(TokensBase tokens)
        {
            _tokens = tokens;
        }
    }
}