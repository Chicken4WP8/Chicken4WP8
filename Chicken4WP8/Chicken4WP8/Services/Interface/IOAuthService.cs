using System.Threading.Tasks;
using Chicken4WP8.Models;
using Chicken4WP8.Models.Setting;

namespace Chicken4WP8.Services.Interface
{
    public interface IOAuthService
    {
        Task<OAuthSessionModel> AuthorizeAsync(string consumerKey, string consumerSecret);
        Task<OAuthSetting> GetTokensAsync(string pinCode);
        Task<IUserModel> VerifyCredentialsAsync();
    }
}
