using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchController
    {
        IEnumerable<ITweet> SearchTweets(string searchQuery);
        IEnumerable<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters);
        IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet);
        IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies);
    }

    public class SearchController : ISearchController
    {
        private readonly ISearchQueryExecutor _searchQueryExecutor;
        private readonly ITweetFactory _tweetFactory;

        public SearchController(
            ISearchQueryExecutor searchQueryExecutor,
            ITweetFactory tweetFactory)
        {
            _searchQueryExecutor = searchQueryExecutor;
            _tweetFactory = tweetFactory;
        }

        public IEnumerable<ITweet> SearchTweets(string searchQuery)
        {
            var tweetsDTO = _searchQueryExecutor.SearchTweets(searchQuery);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        public IEnumerable<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            var tweetsDTO = _searchQueryExecutor.SearchTweets(tweetSearchParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        public IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchRepliesTo(tweet, false);
        }

        public IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            var repliesDTO = _searchQueryExecutor.SearchRepliesTo(tweet.TweetDTO, recursiveReplies);
            return _tweetFactory.GenerateTweetsFromDTO(repliesDTO);
        }
    }
}