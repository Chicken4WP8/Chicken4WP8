using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor
    {
        private static IOAuthCredentials StaticApplicationCredentials { get; set; }

        public CredentialsAccessor()
        {
            CurrentThreadCredentials = StaticApplicationCredentials;
        }

        public IOAuthCredentials ApplicationCredentials
        {
            get { return StaticApplicationCredentials; }
            set
            {
                StaticApplicationCredentials = value;

                if (_currentThreadCredentials == null)
                {
                    _currentThreadCredentials = value;
                }
            }
        }

        private IOAuthCredentials _currentThreadCredentials;
        public IOAuthCredentials CurrentThreadCredentials
        {
            get { return _currentThreadCredentials; }
            set
            {
                _currentThreadCredentials = value;

                if (!HasTheApplicationCredentialsBeenInitialized() && _currentThreadCredentials != null)
                {
                    StaticApplicationCredentials = value;
                }
            }
        }

        #region sync
        public T ExecuteOperationWithCredentials<T>(IOAuthCredentials credentials, Func<T> operation)
        {
            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            var result = operation();
            CurrentThreadCredentials = initialCredentials;
            return result;
        }

        public void ExecuteOperationWithCredentials(IOAuthCredentials credentials, Action operation)
        {
            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            operation();
            CurrentThreadCredentials = initialCredentials;
        }
        #endregion

        #region async
        public async Task<T> ExecuteOperationWithCredentialsAsync<T>(IOAuthCredentials credentials, Func<T> operation)
        {
            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            var result = await Task.Factory.StartNew(() => operation());
            CurrentThreadCredentials = initialCredentials;
            return result;
        }

        public async void ExecuteOperationWithCredentialsAsync(IOAuthCredentials credentials, Action operation)
        {
            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            await Task.Factory.StartNew(() => operation());
            CurrentThreadCredentials = initialCredentials;
        }
        #endregion

        private bool HasTheApplicationCredentialsBeenInitialized()
        {
            return StaticApplicationCredentials != null;
        }
    }
}