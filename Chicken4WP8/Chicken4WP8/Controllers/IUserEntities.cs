using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers
{
    public interface IUserEntities
    {
        IEntities Description { get; set; }
        IEntities Url { get; set; }
    }
}
