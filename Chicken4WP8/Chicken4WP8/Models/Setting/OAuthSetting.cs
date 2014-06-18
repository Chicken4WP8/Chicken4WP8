using Tweetinvi.WebLogic;

namespace Chicken4WP8.Models.Setting
{
    public interface IOAuthSetting
    {
        string Name { get; set; }
        string Description { get; set; }
    }

    /// <summary>
    /// BaseOAuthSetting use OAuthCredentials
    /// </summary>
    public class BaseOAuthSetting : IOAuthSetting
    {
        private string name = "BASE";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = "Authorization with your twitter user name and password";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public OAuthCredentials Credential { get; set; }
    }

    public class TwipOAuthSetting : IOAuthSetting
    {
        private string name = "TWIP4";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = "twip4 mode description";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string BaseUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
