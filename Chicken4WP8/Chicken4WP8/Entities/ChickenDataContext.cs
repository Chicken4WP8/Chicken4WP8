using System.Data.Linq;

namespace Chicken4WP8.Entities
{
    public class ChickenDataContext : DataContext
    {
        private const string connectionString = @"isostore:/ChickenDataContext.sdf";

        public ChickenDataContext()
            : base(connectionString)
        { }

        public Table<Setting> Settings
        {
            get { return this.GetTable<Setting>(); }
        }

        public Table<TempData> TempDatas
        {
            get { return this.GetTable<TempData>(); }
        }

        public Table<CachedUser> CachedUsers
        {
            get { return this.GetTable<CachedUser>(); }
        }

        public Table<CachedImage> CachedImages
        {
            get { return this.GetTable<CachedImage>(); }
        }
    }
}
