using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IListIdentifierFactory
    {
        IListIdentifier Create(ITweetListDTO tweetListDTO);
        IListIdentifier Create(long listId);
        IListIdentifier Create(string slug, IUserIdentifier userDTO);
        IListIdentifier Create(string slug, long ownerId);
        IListIdentifier Create(string slug, string ownerScreenName);
    }
}