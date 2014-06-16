using System;
using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Lists
{
    public class TweetListQueryParameterGenerator : ITweetListQueryParameterGenerator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public TweetListQueryParameterGenerator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public string GenerateIdentifierParameter(IListIdentifier listIdentifier)
        {
            if (listIdentifier.ListId != TweetinviConfig.DEFAULT_ID)
            {
                return String.Format(Resources.List_ListIdParameter, listIdentifier.ListId);
            }

            string ownerIdentifier;
            if (_userQueryValidator.IsUserIdValid(listIdentifier.OwnerId))
            {
                ownerIdentifier = String.Format(Resources.List_OwnerIdParameter, listIdentifier.OwnerId.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                ownerIdentifier = String.Format(Resources.List_OwnerScreenNameParameter, listIdentifier.OwnerScreenName);
            }

            var slugParameter = String.Format(Resources.List_SlugParameter, listIdentifier.Slug);
            return String.Format("{0}&{1}", slugParameter, ownerIdentifier);
        }
    }
}