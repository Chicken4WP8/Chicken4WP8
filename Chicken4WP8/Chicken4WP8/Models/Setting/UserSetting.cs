using System.Collections.Generic;

namespace Chicken4WP8.Models.Setting
{
    public class UserSetting
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public OAuthSetting OAuthSetting { get; set; }
        public HomePageSettings HomePageSettings { get; set; }

        public static UserSetting CreateDefaultUserSettting()
        {
            var setting = new UserSetting
            {
                HomePageSettings = new HomePageSettings
                {
                    Settings = new List<HomePageSetting>
                    {
                        new HomePageSetting{ Index =0, Type = HomePageSettingType.Index },
                        new HomePageSetting{ Index =1,  Type = HomePageSettingType.Mention},
                        new HomePageSetting{Index =2, Type = HomePageSettingType.Message },
                    }
                }
            };
            return setting;
        }
    }
}
