using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Account
{
    public class AccountController : IAccountController
    {
        private readonly IAccountQueryExecutor _accountQueryExecutor;
        private readonly IFactory<IAccountSettings> _accountSettingsUnityFactory;

        public AccountController(
            IAccountQueryExecutor accountQueryExecutor,
            IFactory<IAccountSettings> accountSettingsUnityFactory)
        {
            _accountQueryExecutor = accountQueryExecutor;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
        }

        public IAccountSettings GetLoggedUserSettings()
        {
            var accountSettingsDTO = _accountQueryExecutor.GetLoggedUserAccountSettings();
            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
        }

        private IAccountSettings GenerateAccountSettingsFromDTO(IAccountSettingsDTO accountSettingsDTO)
        {
            var parameterOverride = _accountSettingsUnityFactory.GenerateParameterOverrideWrapper("accountSettingsDTO", accountSettingsDTO);
            return _accountSettingsUnityFactory.Create(parameterOverride);
        }
    }
}