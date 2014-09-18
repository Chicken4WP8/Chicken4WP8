
namespace Chicken4WP8.Models.Setting
{
    public abstract class OAuthSetting
    {
        public abstract string Name { get; }
        public abstract OAuthSettingType OAuthSettingType { get; }
        public abstract string Description { get; }
    }

    public enum OAuthSettingType
    {
        BaseOAuth = 1,
        CustomerOAuth = 2,
        TwipOAuth = 3,
    }

    public class OAuthTypeMetadata
    {
        public OAuthSettingType OAuthType { get; set; }
    }

    public class BaseOAuthSetting : OAuthSetting
    {
        private string name = "BASE";
        public override string Name
        {
            get { return name; }
        }

        public override OAuthSettingType OAuthSettingType
        { get { return OAuthSettingType.BaseOAuth; } }

        private string description = "Authorization with your twitter user name and password";
        public override string Description
        {
            get { return description; }
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class CustomerOAuthSetting : OAuthSetting
    {
        private string name = "CUSTOMER";
        public override string Name
        {
            get { return name; }
        }

        public override OAuthSettingType OAuthSettingType
        { get { return OAuthSettingType.CustomerOAuth; } }

        private string description = "Authorization with customer key and secret";
        public override string Description
        {
            get { return description; }
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class TwipOAuthSetting : OAuthSetting
    {
        private string name = "TWIP4";
        public override string Name
        {
            get { return name; }
        }

        public override OAuthSettingType OAuthSettingType
        {
            get { return Setting.OAuthSettingType.TwipOAuth; }
        }

        private string description = "twip4 mode description";
        public override string Description
        {
            get { return description; }
        }

        public string BaseUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
