using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryHelper
    {
        ITweetSearchParameters CloneTweetSearchParameters(ITweetSearchParameters tweetSearchParameters);
        List<ITweetDTO> GetTweetsFromJsonResponse(string json);
        List<ITweetDTO> GetTweetsFromJsonObject(JObject jObject);
    }

    public class SearchQueryHelper : ISearchQueryHelper
    {
        private readonly IFactory<ITweetSearchParameters> _searchParameterFactory;
        private readonly IJObjectStaticWrapper _jObjectWrapper;

        public SearchQueryHelper(
            IFactory<ITweetSearchParameters> searchParameterFactory,
            IJObjectStaticWrapper jObjectWrapper)
        {
            _searchParameterFactory = searchParameterFactory;
            _jObjectWrapper = jObjectWrapper;
        }

        public ITweetSearchParameters CloneTweetSearchParameters(ITweetSearchParameters tweetSearchParameters)
        {
            var clone = _searchParameterFactory.Create();

            clone.GeoCode = tweetSearchParameters.GeoCode;
            clone.Lang = tweetSearchParameters.Lang;
            clone.Locale = tweetSearchParameters.Locale;
            clone.MaxId = tweetSearchParameters.MaxId;
            clone.SearchType = tweetSearchParameters.SearchType;
            clone.MaximumNumberOfResults = tweetSearchParameters.MaximumNumberOfResults;
            clone.SearchQuery = tweetSearchParameters.SearchQuery;
            clone.SinceId = tweetSearchParameters.SinceId;
            clone.TweetSearchFilter = tweetSearchParameters.TweetSearchFilter;
            clone.Since = tweetSearchParameters.Since;
            clone.Until = tweetSearchParameters.Until;

            return clone;
        }

        public List<ITweetDTO> GetTweetsFromJsonResponse(string json)
        {
            var jObject = _jObjectWrapper.GetJobjectFromJson(json);
            return GetTweetsFromJsonObject(jObject);
        }

        public List<ITweetDTO> GetTweetsFromJsonObject(JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }

            return _jObjectWrapper.ToObject<List<ITweetDTO>>(jObject["statuses"]);
        }
    }
}