using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class CachedImage
    {
        [Column(IsPrimaryKey = true)]
        public string ImageUrl  { get; set; }

        [Column]
        public string Id { get; set; }

        [Column(DbType = "image")]
        public byte[] Data { get; set; }
    }
}
