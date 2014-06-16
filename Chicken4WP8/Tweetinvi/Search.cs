using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi
{
    public static class Search
    {
        [ThreadStatic]
        private static ISearchController _searchController;
        public static ISearchController SearchController
        {
            get
            {
                if (_searchController == null)
                {
                    Initialize();
                }
                
                return _searchController;
            }
        }

        [ThreadStatic]
        private static ISearchQueryParameterGenerator _searchQueryParameterGenerator;
        public static ISearchQueryParameterGenerator SearchQueryParameterGenerator
        {
            get
            {
                if (_searchQueryParameterGenerator == null)
                {
                    Initialize();
                }

                return _searchQueryParameterGenerator;
            }
        }

        static Search()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _searchController = TweetinviContainer.Resolve<ISearchController>();
            _searchQueryParameterGenerator = TweetinviContainer.Resolve<ISearchQueryParameterGenerator>();
        }

        /// <summary>
        /// Search tweets based on the provided search query
        /// </summary>
        public static List<ITweet> SearchTweets(string searchQuery)
        {
            return SearchController.SearchTweets(searchQuery).ToList();
        }

        /// <summary>
        /// Search tweets based on multiple parameters
        /// </summary>
        public static List<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            return SearchController.SearchTweets(tweetSearchParameters).ToList();
        }

        public static IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchController.SearchDirectRepliesTo(tweet);
        }

        public static IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return SearchController.SearchRepliesTo(tweet, recursiveReplies);
        }

        public static ITweetSearchParameters GenerateSearchTweetParameter(string query)
        {
            return SearchQueryParameterGenerator.GenerateSearchTweetParameter(query);
        }

        public static ITweetSearchParameters GenerateSearchTweetParameter(IGeoCode geoCode)
        {
            return SearchQueryParameterGenerator.GenerateSearchTweetParameter(geoCode);
        }

        public static ITweetSearchParameters GenerateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.GenerateSearchTweetParameter(coordinates, radius, measure);
        }

        public static ITweetSearchParameters GenerateSearchTweetParameter(double longitude, double latitude, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.GenerateSearchTweetParameter(longitude, latitude, radius, measure);   
        }
    }
}