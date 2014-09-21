using System.Collections.Generic;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.Models.Tombstoning
{
    public class IndexViewTombstoningData : TombstoningDataBase
    {
        public List<ITweetModel> Tweets { get; set; }
    }

    public class MentionViewTombstoningData : TombstoningDataBase
    {
        public List<ITweetModel> Mentions { get; set; }
    }
}
