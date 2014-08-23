using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IDirectMessageController
    {
        Task<IEnumerable<IDirectMessageModel>> SentAsync(IDictionary<string, object> parameters);
        Task<IEnumerable<IDirectMessageModel>> ReceivedAsync(IDictionary<string, object> parameters);
        Task<IDirectMessageModel> NewAsync(IDictionary<string, object> parameters);
        Task<IDirectMessageModel> DestroyAsync(IDictionary<string, object> parameters);
    }
}
