using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class Setting
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, DbType = "int IDENTITY(1,1)")]
        public int Id { get; set; }

        [Column]
        public bool IsCurrentlyInUsed { get; set; }

        [Column]
        public SettingCategory Category { get; set; }

        [Column]
        public string Name { get; set; }

        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] Data { get; set; }
    }

    public enum SettingCategory
    {
        OAuthSetting = 100,
        LanguageSetting = 200,
        CurrentUserSetting = 300,
    }
}
