
namespace Tweetinvi.Core.Interfaces
{
    public interface IResourcesManager
    {
        string BaseUrl { get; set; }

        string OAuth_PINCode_CallbackURL { get; }

        string OAuthRequestAccessToken { get; }

        string OAuthRequestToken { get; }

        string OAuthTokenAccessRegex { get; }

        string OAuthTokenRequestRegex { get; }

        string OAuthRequestAuthorize { get; }

        string OAuthToken_GetVerifierCode_Regex { get; }

        string OAuthRequestAccessTokenWithUserCredentials { get; }

        string TokenUser_GetCurrentUser { get; }

        string User_GetUserFromId { get; }

        string User_GetUserFromName { get; }

        string User_GetUsersFromIds { get; }

        string User_GetUsersFromNames { get; }
    }
}
