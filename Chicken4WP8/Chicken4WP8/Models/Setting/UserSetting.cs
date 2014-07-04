
namespace Chicken4WP8.Models.Setting
{
    public class UserSetting
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public OAuthSetting OAuthSetting { get; set; }
    }
}
