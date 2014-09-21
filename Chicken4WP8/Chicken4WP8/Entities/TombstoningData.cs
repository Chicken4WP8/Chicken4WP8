using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class TombstoningData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
        public int Id { get; set; }

        [Column]
        public TombstoningType Type { get; set; }

        [Column]
        public string Key { get; set; }

        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] Data { get; set; }
    }

    public enum TombstoningType
    {
        HomePageView = 100,
        IndexView = 101,
    }
}
