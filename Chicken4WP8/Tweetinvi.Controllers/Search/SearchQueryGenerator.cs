using System.Text;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryGenerator
    {
        string GetSearchTweetsQuery(string query);
        string GetSearchTweetsQuery(ITweetSearchParameters tweetSearchParameters);
    }

    public class SearchQueryGenerator : ISearchQueryGenerator
    {
        private readonly ISearchQueryValidator _searchQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ISearchQueryParameterGenerator _searchQueryParameterGenerator;

        public SearchQueryGenerator(
            ISearchQueryValidator searchQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ISearchQueryParameterGenerator searchQueryParameterGenerator)
        {
            _searchQueryValidator = searchQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _searchQueryParameterGenerator = searchQueryParameterGenerator;
        }

        public string GetSearchTweetsQuery(string query)
        {
            var searchParameter = _searchQueryParameterGenerator.GenerateSearchTweetParameter(query);
            return GetSearchTweetsQuery(searchParameter);
        }

        public string GetSearchTweetsQuery(ITweetSearchParameters tweetSearchParameters)
        {
            if (!_searchQueryValidator.IsSearchParameterValid(tweetSearchParameters) ||
                !_searchQueryValidator.IsSearchQueryValid(tweetSearchParameters.SearchQuery))
            {
                return null;
            }

            StringBuilder query = new StringBuilder();

            query.Append(_searchQueryParameterGenerator.GenerateSearchQueryParameter(tweetSearchParameters.SearchQuery));
            query.Append(_searchQueryParameterGenerator.GenerateSearchTypeParameter(tweetSearchParameters.SearchType));

            query.Append(_queryParameterGenerator.GenerateSinceIdParameter(tweetSearchParameters.SinceId));
            query.Append(_queryParameterGenerator.GenerateMaxIdParameter(tweetSearchParameters.MaxId));
            query.Append(_queryParameterGenerator.GenerateCountParameter(tweetSearchParameters.MaximumNumberOfResults));

            query.Append(_searchQueryParameterGenerator.GenerateGeoCodeParameter(tweetSearchParameters.GeoCode));
            query.Append(_searchQueryParameterGenerator.GenerateLangParameter(tweetSearchParameters.Lang));
            query.Append(_searchQueryParameterGenerator.GenerateLocaleParameter(tweetSearchParameters.Locale));
            query.Append(_searchQueryParameterGenerator.GenerateSinceParameter(tweetSearchParameters.Since));
            query.Append(_searchQueryParameterGenerator.GenerateUntilParameter(tweetSearchParameters.Until));

            return query.ToString();
        }
    }
}