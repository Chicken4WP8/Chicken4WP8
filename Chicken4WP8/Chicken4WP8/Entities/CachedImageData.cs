using System.Data.Linq.Mapping;

namespace Chicken4WP8.Entities
{
    [Table]
    public class CachedImage
    {
        //[Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "bigint IDENTITY(1,1)")]
        //public long Key { get; set; }

       [Column(IsPrimaryKey = true)]
        public string Key  { get; set; }

        /// <summary>
        /// when user updated profile image,
        /// the ImageUrl changed,
        /// but userId not.
        /// </summary>
        [Column]
        public string Id { get; set; }

        [Column(DbType = "image")]
        public byte[] Data { get; set; }
    }
}
