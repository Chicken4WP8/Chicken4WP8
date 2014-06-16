using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IFriendshipFactory
    {
        // Generate from DTO
        IRelationship GenerateRelationshipFromRelationshipDTO(IRelationshipDTO relationshipDTO);
        IEnumerable<IRelationship> GenerateRelationshipsFromRelationshipsDTO(IEnumerable<IRelationshipDTO> relationshipDTO);

        // Generate RelationshipAuthorizations
        IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool deviceNotificationEnabled);
    }
}