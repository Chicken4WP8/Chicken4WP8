using System.Collections.Generic;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.Models.Tombstoning
{
    public class IndexPageTombstoningData:TombstoningDataBase
    {
        public List<ITweetModel> Tweets { get; set; }
    }
}
