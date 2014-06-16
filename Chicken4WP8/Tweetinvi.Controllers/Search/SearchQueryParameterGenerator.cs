using System;
using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryParameterGenerator
    {
        ITweetSearchParameters GenerateSearchTweetParameter(string query);
        ITweetSearchParameters GenerateSearchTweetParameter(IGeoCode geoCode);
        ITweetSearchParameters GenerateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure);
        ITweetSearchParameters GenerateSearchTweetParameter(double longitude, double latitude, int radius, DistanceMeasure measure);

        string GenerateSearchQueryParameter(string query);
        string GenerateSearchTypeParameter(SearchResultType searchType);
        string GenerateSinceParameter(DateTime since);
        string GenerateUntilParameter(DateTime until);
        string GenerateLocaleParameter(string locale);
        string GenerateLangParameter(Language lang);
        string GenerateGeoCodeParameter(IGeoCode geoCode);
    }

    public class SearchQueryParameterGenerator : ISearchQueryParameterGenerator
    {
        private readonly ISearchQueryValidator _searchQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;
        private readonly IFactory<ITweetSearchParameters> _tweetSearchParameterFactory;

        public SearchQueryParameterGenerator(
            ISearchQueryValidator searchQueryValidator, 
            ITwitterStringFormatter twitterStringFormatter,
            IFactory<ITweetSearchParameters> tweetSearchParameterFactory)
        {
            _searchQueryValidator = searchQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
            _tweetSearchParameterFactory = tweetSearchParameterFactory;
        }

        public ITweetSearchParameters GenerateSearchTweetParameter(string query)
        {
            var searchParameter = _tweetSearchParameterFactory.Create();
            searchParameter.SearchQuery = query;
            return searchParameter;
        }

        public ITweetSearchParameters GenerateSearchTweetParameter(IGeoCode geoCode)
        {
            var searchParameter = _tweetSearchParameterFactory.Create();
            searchParameter.GeoCode = geoCode;
            return searchParameter;
        }

        public ITweetSearchParameters GenerateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            var searchParameter = _tweetSearchParameterFactory.Create();
            searchParameter.SetGeoCode(coordinates, radius, measure);
            return searchParameter;
        }

        public ITweetSearchParameters GenerateSearchTweetParameter(double longitude, double latitude, int radius, DistanceMeasure measure)
        {
            var searchParameter = _tweetSearchParameterFactory.Create();
            searchParameter.SetGeoCode(longitude, latitude, radius, measure);
            return searchParameter;
        }

        public string GenerateSearchQueryParameter(string searchQuery)
        {
            var formattedQuery = _twitterStringFormatter.TwitterEncode(searchQuery);
            return String.Format(Resources.Search_SearchTweets, formattedQuery);
        }

        public string GenerateSearchTypeParameter(SearchResultType searchType)
        {
            return String.Format(Resources.SearchParameter_ResultType, searchType);
        }

        public string GenerateSinceParameter(DateTime since)
        {
            if (!_searchQueryValidator.IsDateTimeDefined(since))
            {
                return String.Empty;
            }

            return String.Format(Resources.SearchParameter_Since, since.ToString("yyyy-MM-dd"));
        }

        public string GenerateUntilParameter(DateTime until)
        {
            if (!_searchQueryValidator.IsDateTimeDefined(until))
            {
                return String.Empty;
            }

            return String.Format(Resources.SearchParameter_Until, until.ToString("yyyy-MM-dd"));
        }

        public string GenerateLocaleParameter(string locale)
        {
            if (!_searchQueryValidator.IsLocaleParameterValid(locale))
            {
                return String.Empty;
            }

            return locale;
        }

        public string GenerateLangParameter(Language lang)
        {
            if (!_searchQueryValidator.IsLangDefined(lang))
            {
                return String.Empty;
            }

            return String.Format(Resources.SearchParameter_Lang, lang.GetDescriptionAttribute());
        }

        public string GenerateGeoCodeParameter(IGeoCode geoCode)
        {
            if (!_searchQueryValidator.IsGeoCodeValid(geoCode))
            {
                return String.Empty;
            }

            string latitude = geoCode.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            string longitude = geoCode.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            string radius = geoCode.Radius.ToString(CultureInfo.InvariantCulture);
            string measure = geoCode.DistanceMeasure == DistanceMeasure.Kilometers ? "km" : "mi";
            return String.Format(Resources.SearchParameter_GeoCode, latitude, longitude, radius, measure, CultureInfo.InvariantCulture);
        }
    }
}