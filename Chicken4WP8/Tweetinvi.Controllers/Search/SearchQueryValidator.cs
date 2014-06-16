using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryValidator
    {
        bool IsSearchParameterValid(ITweetSearchParameters searchParameters);
        bool IsSearchQueryValid(string searchQuery);
        bool IsGeoCodeValid(IGeoCode geoCode);
        bool IsLocaleParameterValid(string locale);
        bool IsLangDefined(Language lang);
        bool IsDateTimeDefined(DateTime untilDateTime);
    }

    public class SearchQueryValidator : ISearchQueryValidator
    {
        public bool IsSearchParameterValid(ITweetSearchParameters searchParameters)
        {
            return searchParameters != null && IsAtLeasOneRequiredCriteriaSet(searchParameters);
        }

        private bool IsAtLeasOneRequiredCriteriaSet(ITweetSearchParameters searchParameters)
        {
            bool isSearchQuerySet = !String.IsNullOrEmpty(searchParameters.SearchQuery);
            bool isSearchQueryValid = IsSearchQueryValid(searchParameters.SearchQuery);
            bool isGeoCodeSet = IsGeoCodeValid(searchParameters.GeoCode);

            return (isSearchQuerySet && isSearchQueryValid) || isGeoCodeSet;
        }

        public bool IsSearchQueryValid(string searchQuery)
        {
            // We might want to restrict the size to 1000 characters as indicated in the documentation
            return true;
        }

        public bool IsGeoCodeValid(IGeoCode geoCode)
        {
            return geoCode != null;
        }

        public bool IsLocaleParameterValid(string locale)
        {
            return !String.IsNullOrEmpty(locale);
        }

        public bool IsLangDefined(Language lang)
        {
            return lang != Language.Undefined;
        }

        public bool IsDateTimeDefined(DateTime dateTime)
        {
            return dateTime != default (DateTime);
        }
    }
}