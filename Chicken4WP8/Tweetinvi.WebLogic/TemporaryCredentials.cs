using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.WebLogic
{
    public class TemporaryCredentials : ConsumerCredentials, ITemporaryCredentials
    {
        public TemporaryCredentials(string consumerKey, string consumerSecret) 
            : base(consumerKey, consumerSecret)
        {
        }

        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }
        public string VerifierCode { get; set; }
    }
}