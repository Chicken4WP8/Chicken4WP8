using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        IEnumerable<ITweetDTO> SearchTweets(string query);
        IEnumerable<ITweetDTO> SearchTweets(ITweetSearchParameters tweetSearchParameters);
        IEnumerable<ITweetDTO> SearchRepliesTo(ITweetDTO tweetDTO, bool getRecursiveReplies);
    }

    public class SearchQueryExecutor : ISearchQueryExecutor
    {
        private readonly ISearchQueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ISearchQueryHelper _searchQueryHelper;
        private readonly ITweetHelper _tweetHelper;

        public SearchQueryExecutor(
            ISearchQueryGenerator searchQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ISearchQueryHelper searchQueryHelper,
            ITweetHelper tweetHelper)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _searchQueryHelper = searchQueryHelper;
            _tweetHelper = tweetHelper;
        }

        public IEnumerable<ITweetDTO> SearchTweets(string searchQuery)
        {
            string httpQuery = _searchQueryGenerator.GetSearchTweetsQuery(searchQuery);
            return GetTweetDTOsFromSearch(httpQuery);
        }

        public IEnumerable<ITweetDTO> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            if (tweetSearchParameters == null)
            {
                throw new ArgumentException("TweetSearch Parameters cannot be null");
            }

            IEnumerable<ITweetDTO> result;
            if (tweetSearchParameters.MaximumNumberOfResults > 100)
            {
                result = SearchTweetsRecursively(tweetSearchParameters);
            }
            else
            {
                string httpQuery = _searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);
                result = GetTweetDTOsFromSearch(httpQuery);
            }
            
            if (tweetSearchParameters.TweetSearchFilter == TweetSearchFilter.OriginalTweetsOnly)
            {
                return result.Where(x => x.RetweetedTweetDTO == null);
            }

            if (tweetSearchParameters.TweetSearchFilter == TweetSearchFilter.RetweetsOnly)
            {
                return result.Where(x => x.RetweetedTweetDTO != null);
            }

            return result;
        }

        private IEnumerable<ITweetDTO> SearchTweetsRecursively(ITweetSearchParameters tweetSearchParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(tweetSearchParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 100);

            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);
            var currentResult = GetTweetDTOsFromSearch(query);
            List<ITweetDTO> result = currentResult;

            while (result.Count < tweetSearchParameters.MaximumNumberOfResults)
            {
                if (currentResult.IsEmpty())
                {
                    // If Twitter does not any result left, stop the search
                    break;
                }

                var oldestTweetId = _tweetHelper.GetOldestTweetId(currentResult);
                searchParameter.MaxId = oldestTweetId;
                searchParameter.MaximumNumberOfResults = Math.Min(tweetSearchParameters.MaximumNumberOfResults - result.Count, 100);
                query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);
                currentResult = GetTweetDTOsFromSearch(query);
                result.AddRange(currentResult);
            }

            return result;
        }

        public IEnumerable<ITweetDTO> SearchRepliesTo(ITweetDTO tweetDTO, bool recursiveReplies)
        {
            if (tweetDTO == null)
            {
                throw new ArgumentException("TweetDTO cannot be null");
            }

            var searchTweets = SearchTweets(String.Format(tweetDTO.Creator.ScreenName)).ToList();

            if (recursiveReplies)
            {
                return GetRecursiveReplies(searchTweets, tweetDTO.Id);
            }

            var repliesDTO = searchTweets.Where(x => x.InReplyToStatusId == tweetDTO.Id);
            return repliesDTO;
        }

        private IEnumerable<ITweetDTO> GetRecursiveReplies(List<ITweetDTO> searchTweets, long sourceId)
        {
            var directReplies = searchTweets.Where(x => x.InReplyToStatusId == sourceId).ToList();
            List<ITweetDTO> results = directReplies.ToList();

            var recursiveReplies = searchTweets.Where(x => directReplies.Select(r => r.Id as long?).Contains(x.InReplyToStatusId));
            results.AddRange(recursiveReplies);

            while (recursiveReplies.Any())
            {
                var repliesFromPreviousLevel = recursiveReplies;
                recursiveReplies = searchTweets.Where(x => repliesFromPreviousLevel.Select(r => r.Id as long?).Contains(x.InReplyToStatusId));
                results.AddRange(recursiveReplies);
            }
            
            return results;
        }

        private List<ITweetDTO> GetTweetDTOsFromSearch(string query)
        {
            var jObject = _twitterAccessor.ExecuteGETQuery(query);
            return _searchQueryHelper.GetTweetsFromJsonObject(jObject);
        }
    }
}