using Tweetinvi.Controllers.Properties;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryGenerator
    {
        string GetLoggedUserAccountSettingsQuery();
    }

    public class AccountQueryGenerator : IAccountQueryGenerator
    {
        public string GetLoggedUserAccountSettingsQuery()
        {
            return Resources.Account_GetSettings;
        }
    }
}