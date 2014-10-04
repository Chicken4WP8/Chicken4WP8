using System.Collections.Generic;

namespace Chicken4WP8.Models.Setting
{
    public class HomePageSettings
    {
        public List<HomePageSetting> Settings { get; set; }
    }

    public enum HomePageSettingType
    {
        Index = 0,
        Mention = 1,
        Message = 2,
    }

    public class HomePageSettingTypeMetadata
    {
        public HomePageSettingType Type { get; set; }
    }

    public class HomePageSetting
    {
        public HomePageSettingType Type { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
    }
}
