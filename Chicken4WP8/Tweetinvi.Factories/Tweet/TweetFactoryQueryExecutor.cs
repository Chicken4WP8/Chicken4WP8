using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Factories.Properties;
using Tweetinvi.Logic.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public interface ITweetFactoryQueryExecutor
    {
        ITweetDTO GetTweetDTO(long tweetId);
        ITweetDTO CreateTweetDTO(string text);
    }

    public class TweetFactoryQueryExecutor : ITweetFactoryQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<ITweetDTO> _tweetDTOUnityFactory;

        public TweetFactoryQueryExecutor(ITwitterAccessor twitterAccessor, IFactory<ITweetDTO> tweetDTOUnityFactory)
        {
            _twitterAccessor = twitterAccessor;
            _tweetDTOUnityFactory = tweetDTOUnityFactory;
        }

        public ITweetDTO GetTweetDTO(long tweetId)
        {
            string query = String.Format(Resources.Tweet_Get, tweetId);
            return _twitterAccessor.ExecuteGETQuery<TweetDTO>(query);
        }

        public ITweetDTO CreateTweetDTO(string text)
        {
            var tweetDTO = _tweetDTOUnityFactory.Create();
            tweetDTO.Text = text;

            return tweetDTO;
        }
    }
}