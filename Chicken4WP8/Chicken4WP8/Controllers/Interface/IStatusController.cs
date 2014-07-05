using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IStatusController
    {
        Task<IEnumerable<ITweetModel>> HomeTimelineAsync(IDictionary<string, object> parameters=null);
    }
}
