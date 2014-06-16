using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface ITweetListQueryParameterGenerator
    {
        string GenerateIdentifierParameter(IListIdentifier listIdentifier);
    }
}