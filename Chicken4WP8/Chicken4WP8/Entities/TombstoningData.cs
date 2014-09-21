using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class TombstoningData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
        public int PrimaryKey { get; set; }

        [Column]
        public TombstoningType Type { get; set; }

        [Column]
        public string Id { get; set; }

        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] Data { get; set; }
    }

    public enum TombstoningType
    {
        HomePage = 100,
        IndexView = 101,
        MentionView = 102,
    }
}
