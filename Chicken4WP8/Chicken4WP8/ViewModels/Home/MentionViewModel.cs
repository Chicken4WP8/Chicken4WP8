using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Controllers;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Home
{
    public class MentionViewModel : IndexViewModel
    {
        #region properties

        public MentionViewModel(
            ILanguageHelper languageHelper,
            IEnumerable<Lazy<IStatusController, OAuthTypeMetadata>> statusControllers,
            IEnumerable<Lazy<IUserController, OAuthTypeMetadata>> userControllers)
            : base(languageHelper, statusControllers, userControllers)
        { }
        #endregion

        protected override void SetLanguage()
        {
            DisplayName = LanguageHelper["MentionViewModel_Header"];
        }

        protected override async Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options)
        {
            var tweets = await statusController.MentionsTimelineAsync(options);
            if (tweets != null)
                return tweets.ToList();
            return null;
        }
    }
}
