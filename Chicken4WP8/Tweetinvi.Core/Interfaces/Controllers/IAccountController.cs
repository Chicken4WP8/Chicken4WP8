using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IAccountController
    {
        IAccountSettings GetLoggedUserSettings();
    }
}