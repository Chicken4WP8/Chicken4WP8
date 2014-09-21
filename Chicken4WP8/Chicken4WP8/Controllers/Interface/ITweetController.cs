using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface ITweetController
    {
        Task<IEnumerable<ITweetModel>> HomeTimelineAsync(IDictionary<string, object> parameters = null);
        Task<IEnumerable<ITweetModel>> MentionsTimelineAsync(IDictionary<string, object> parameters = null);
        Task<ITweetModel> ShowAsync(IDictionary<string, object> parameters);
        Task SetStatusImagesAsync(ITweetModel status);
        Task<ITweetModel> UpdateAsync(IDictionary<string, object> parameters);
    }
}
