using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Interfaces.QueryValidators
{
    public interface ITweetListQueryValidator
    {
        // ListParameter
        bool IsListUpdateParametersValid(IListUpdateParameters parameters);
        
        // Parameters
        bool IsDescriptionParameterValid(string description);
        bool IsNameParameterValid(string name);
        
        // Identifiers
        bool IsListIdentifierValid(IListIdentifier parameters);
        bool IsOwnerScreenNameValid(string ownerScreeName);
        bool IsOwnerIdValid(long ownerId);
    }
}