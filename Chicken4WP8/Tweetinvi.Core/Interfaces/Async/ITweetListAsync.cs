using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITweetListAsync
    {
        Task<IEnumerable<ITweet>> GetTweetsAsync();
        Task<IEnumerable<IUser>> GetMembersAsync(int maxNumberOfUsersToRetrieve = 100);
        Task<bool> UpdateAsync(IListUpdateParameters parameters);
        Task<bool> DestroyAsync();
    }
}
