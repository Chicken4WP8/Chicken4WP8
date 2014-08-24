using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
        public class CachedDirectMessage
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
            public long PrimaryKey { get; set; }

            [Column]
            public long Id { get; set; }

            [Column]
            public long UserId { get; set; }

            [Column]
            public bool IsSentByMe { get; set; }

            [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
            public byte[] Data { get; set; }
        }    
}
