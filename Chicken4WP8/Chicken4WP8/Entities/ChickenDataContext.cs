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

        public Table<CachedTweet> CachedTweets
        {
            get
            {
                return GetTable<CachedTweet>();
            }
        }

        public Table<CachedUser> CachedUsers
        {
            get { return this.GetTable<CachedUser>(); }
        }

        public Table<CachedImage> CachedImages
        {
            get { return this.GetTable<CachedImage>(); }
        }

        public Table<CachedDirectMessage> CachedDirectMessages
        {
            get
            {
                return GetTable<CachedDirectMessage>();
            }
        }

        public Table<TombstoningData> TombstoningDatas
        {
            get { return GetTable<TombstoningData>(); }
        }
    }
}
