using System.Threading.Tasks;
using Chicken4WP8.Models;
using Chicken4WP8.Models.Setting;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IOAuthController
    {
        Task<OAuthSessionModel> AuthorizeAsync(string consumerKey, string consumerSecret);
        Task<OAuthSetting> GetTokensAsync(string pinCode);
        Task<IUserModel> VerifyCredentialsAsync(OAuthSetting setting);
    }
}