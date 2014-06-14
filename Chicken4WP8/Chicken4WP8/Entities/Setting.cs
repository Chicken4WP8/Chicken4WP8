using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class Setting
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, DbType = "int IDENTITY(1,1)")]
        public int Id { get; set; }

        [Column]
        public SettingCategory Category { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public bool IsCurrentlyInUsed { get; set; }

        [Column]
        public string Data { get; set; }
    }

    public enum SettingCategory
    {
        OAuthSetting = 100,
        LanguageSetting = 200,
        CurrentUser = 300,
    }
}
