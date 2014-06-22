using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IWebTokenCreatorAsync
    {
       Task<string> GetPinCodeAuthorizationURLAsync(ITemporaryCredentials temporaryCredentials);
       Task<string> GetAuthorizationURLAsync(ITemporaryCredentials temporaryCredentials, string callbackURL);
    }

    public interface IWebTokenCreator : IWebTokenCreatorAsync
    {
        string GetPinCodeAuthorizationURL(ITemporaryCredentials temporaryCredentials);
        string GetAuthorizationURL(ITemporaryCredentials temporaryCredentials, string callbackURL);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}