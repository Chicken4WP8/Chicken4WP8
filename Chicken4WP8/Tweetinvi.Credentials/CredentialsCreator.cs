using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Credentials
{
    public interface ICredentialsCreatorAsync
    {
        Task<IOAuthCredentials> GetCredentialsFromVerifierCodeAsync(string verifierCode, ITemporaryCredentials temporaryCredentials);
    }

    public interface ICredentialsCreator : ICredentialsCreatorAsync
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
        private readonly IResourcesManager _resourcesManager;

        public CredentialsCreator(
            ICredentialsFactory credentialsFactory,
            ITwitterRequester twitterRequester,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IFactory<ITemporaryCredentials> applicationCredentialsUnityFactory,
            IResourcesManager resourcesManager)
        {
            _credentialsFactory = credentialsFactory;
            _twitterRequester = twitterRequester;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _applicationCredentialsUnityFactory = applicationCredentialsUnityFactory;
            _resourcesManager = resourcesManager;
        }

        #region sync
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
            var response = _twitterRequester.ExecuteQueryWithTemporaryCredentials(_resourcesManager.OAuthRequestAccessToken, HttpMethod.POST, temporaryCredentials: temporaryCredentials, headers: new[] { callbackParameter });

            if (response == null)
            {
                return null;
            }

            Match responseInformation = Regex.Match(response, _resourcesManager.OAuthTokenAccessRegex);
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
        #endregion

        #region async
        public async Task<IOAuthCredentials> GetCredentialsFromVerifierCodeAsync(string verifierCode, ITemporaryCredentials temporaryCredentials)
        {
            var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true, true, false);
            var response = await _twitterRequester.ExecuteQueryWithTemporaryCredentialsAsync(_resourcesManager.OAuthRequestAccessToken, HttpMethod.POST, temporaryCredentials: temporaryCredentials, headers: new[] { callbackParameter });

            if (response == null)
            {
                return null;
            }

            Match responseInformation = Regex.Match(response, _resourcesManager.OAuthTokenAccessRegex);
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
        #endregion
    }
}