using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Factories.Properties;

namespace Tweetinvi.Factories.Lists
{
    public interface ITweetListFactoryQueryGenerator
    {
        string GetCreateListQuery(string name, PrivacyMode privacyMode, string description);
        string GetListByIdQuery(IListIdentifier listIdentifier);
    }

    public class TweetListFactoryQueryGenerator : ITweetListFactoryQueryGenerator
    {
        private readonly ITweetListQueryValidator _listsQueryValidator;
        private readonly ITweetListQueryParameterGenerator _listQueryParameterGenerator;

        public TweetListFactoryQueryGenerator(
            ITweetListQueryValidator listsQueryValidator,
            ITweetListQueryParameterGenerator listQueryParameterGenerator)
        {
            _listsQueryValidator = listsQueryValidator;
            _listQueryParameterGenerator = listQueryParameterGenerator;
        }

        public string GetCreateListQuery(string name, PrivacyMode privacyMode, string description)
        {
            var baseQuery = String.Format(Resources.List_Create, name, privacyMode.ToString().ToLower());

            if (_listsQueryValidator.IsDescriptionParameterValid(description))
            {
                baseQuery += String.Format(Resources.List_Create_DescriptionParameter, description);
            }

            return baseQuery;
        }

        public string GetListByIdQuery(IListIdentifier listIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier))
            {
                return null;
            }

            var identifierParameter = _listQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return String.Format(Resources.List_GetExistingList, identifierParameter);
        }
    }
}