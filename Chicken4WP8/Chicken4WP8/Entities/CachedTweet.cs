using System;
using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class CachedTweet
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
        public long PrimaryKey { get; set; }

        [Column]
        public long Id { get; set; }

        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] Data { get; set; }
    }
}
