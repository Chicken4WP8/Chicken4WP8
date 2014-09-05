using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class TempData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
        public int Id { get; set; }

        [Column]
        public TempType Type { get; set; }

        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] Data { get; set; }
    }

    public enum TempType
    {
        TweetDetail = 1,
        UserProfile = 2,
        DirectMessage = 3,
        NewStatus = 4,
    }
}
