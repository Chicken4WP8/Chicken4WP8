using System.Collections.Generic;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.Models.Tombstoning
{
    public class IndexViewTombstoningData : TombstoningDataBase
    {
        public List<ITweetModel> Tweets { get; set; }
        public List<ITweetModel> FetchedItemsCache { get; set; }
        public List<ITweetModel> MissedItemsCache { get; set; }
        public List<ITweetModel> LoadedItemsCache { get; set; }
    }

    public class MentionViewTombstoningData : TombstoningDataBase
    {
        public List<ITweetModel> Mentions { get; set; }
        public List<ITweetModel> FetchedItemsCache { get; set; }
        public List<ITweetModel> MissedItemsCache { get; set; }
        public List<ITweetModel> LoadedItemsCache { get; set; }
    }
}
