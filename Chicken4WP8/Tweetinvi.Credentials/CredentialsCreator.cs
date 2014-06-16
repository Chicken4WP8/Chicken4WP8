using System.Text.RegularExpressions;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Credentials.Properties;

namespace Tweetinvi.Credentials
{
    public interface ICredentialsCreator
    {
        ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret);
        IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials);
    }

    public class CredentialsCreator : ICredentialsCreator
    {
        private readonly ICredentialsFactory _credentialsFactory;
        private readonly ITwitterRequester _twitterRequester;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly IFactory<ITemporaryCredentials> _applicationCredentialsUnityFactory;

        public CredentialsCreator(
            ICredentialsFactory credentialsFactory,
            ITwitterRequester twitterRequester,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IFactory<ITemporaryCredentials> applicationCredentialsUnityFactory)
        {
            _credentialsFactory = credentialsFactory;
            _twitterRequester = twitterRequester;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _applicationCredentialsUnityFactory = applicationCredentialsUnityFactory;
        }

        // Step 0 - Generate Temporary Credentials
        public ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret)
        {
            var consumerKeyParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerKey", consumerKey);
            var consumerSecretParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerSecret", consumerSecret);

            return _applicationCredentialsUnityFactory.Create(consumerKeyParameterOverride, consumerSecretParameterOverride);
        }

        // Step 2 - Generate User Credentials
        public IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials)
        {
            var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true, true, false);
            var response = _twitterRequester.ExecuteQueryWithTemporaryCredentials(Resources.OAuthRequestAccessToken, HttpMethod.POST, temporaryCredentials: temporaryCredentials, headers: new[] { callbackParameter });

            if (response == null)
            {
                return null;
            }

            Match responseInformation = Regex.Match(response, Resources.OAuthTokenAccessRegex);
            if (responseInformation.Groups["oauth_token"] == null || responseInformation.Groups["oauth_token_secret"] == null)
            {
                return null;
            }

            var credentials = _credentialsFactory.CreateOAuthCredentials(
                    responseInformation.Groups["oauth_token"].Value,
                    responseInformation.Groups["oauth_token_secret"].Value,
                    temporaryCredentials.ConsumerKey,
                    temporaryCredentials.ConsumerSecret);

            return credentials;
        }
    }
}