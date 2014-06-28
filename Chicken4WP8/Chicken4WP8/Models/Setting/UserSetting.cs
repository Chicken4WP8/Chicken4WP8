using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8.Models.Setting
{
    public class UserSetting
    {
        public ILoggedUser LoggedUser { get; set; }
        public OAuthSetting OAuthSetting { get; set; }
    }
}
