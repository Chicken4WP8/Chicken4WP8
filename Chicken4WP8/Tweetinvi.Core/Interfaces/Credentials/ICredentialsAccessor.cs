using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsAccessorAsync
    {
        Task<T> ExecuteOperationWithCredentialsAsync<T>(IOAuthCredentials credentials, Func<T> operation);
        void ExecuteOperationWithCredentialsAsync(IOAuthCredentials credentials, Action operation);
    }

    public interface ICredentialsAccessor : ICredentialsAccessorAsync
    {
        IOAuthCredentials ApplicationCredentials { get; set; }
        IOAuthCredentials CurrentThreadCredentials { get; set; }

        T ExecuteOperationWithCredentials<T>(IOAuthCredentials credentials, Func<T> operation);
        void ExecuteOperationWithCredentials(IOAuthCredentials credentials, Action operation);
    }
}