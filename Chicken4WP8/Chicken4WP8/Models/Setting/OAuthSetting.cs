
namespace Chicken4WP8.Models.Setting
{
    public abstract class OAuthSetting
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }

    /// <summary>
    /// BaseOAuthSetting use OAuthCredentials
    /// </summary>
    public class BaseOAuthSetting : OAuthSetting
    {
        private string name = "BASE";
        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = "Authorization with your twitter user name and password";
        public override string Description
        {
            get { return description; }
            set { description = value; }
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
            set { name = value; }
        }

        private string description = "twip4 mode description";
        public override string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string BaseUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
