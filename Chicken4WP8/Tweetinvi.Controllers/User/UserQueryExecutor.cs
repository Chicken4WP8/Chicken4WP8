using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.User
{
    public interface IUserQueryExecutorAsync
    {
        // Friend Ids
        Task<IEnumerable<long>> GetFriendIdsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve);
        Task<IEnumerable<long>> GetFriendIdsAsync(long userId, int maxFriendsToRetrieve);
        Task<IEnumerable<long>> GetFriendIdsAsync(string userScreenName, int maxFriendsToRetrieve);

        // Followers Ids
        Task<IEnumerable<long>> GetFollowerIdsAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve);
        Task<IEnumerable<long>> GetFollowerIdsAsync(long userId, int maxFollowersToRetrieve);
        Task<IEnumerable<long>> GetFollowerIdsAsync(string userScreenName, int maxFollowersToRetrieve);

        // Favourites
        Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(IUserIdentifier userDTO, int maxFavouritesToRetrieve);
        Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(long userId, int maxFavouritesToRetrieve);
        Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(string userScreenName, int maxFavouritesToRetrieve);

        // Block User
        Task<bool> BlockUserAsync(IUserIdentifier userDTO);
        Task<bool> BlockUserAsync(long userId);
        Task<bool> BlockUserAsync(string userScreenName);

        // Stream Profile Image
        Task<Stream> GetProfileImageStreamAsync(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
    }

    public interface IUserQueryExecutor : IUserQueryExecutorAsync
    {
        // Friend Ids
        IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve);
        IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve);
        IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve);

        // Followers Ids
        IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve);
        IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve);
        IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve);

        // Favourites
        IEnumerable<ITweetDTO> GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve);
        IEnumerable<ITweetDTO> GetFavouriteTweets(long userId, int maxFavouritesToRetrieve);
        IEnumerable<ITweetDTO> GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve);

        // Block User
        bool BlockUser(IUserIdentifier userDTO);
        bool BlockUser(long userId);
        bool BlockUser(string userScreenName);

        // Stream Profile Image
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
    }

    public class UserQueryExecutor : IUserQueryExecutor
    {
        private readonly IUserQueryGenerator _userQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IWebHelper _webHelper;
        public UserQueryExecutor(
            IUserQueryGenerator userQueryGenerator,
            ITwitterAccessor twitterAccessor,
            IWebHelper webHelper)
        {
            _userQueryGenerator = userQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _webHelper = webHelper;
        }

        #region sync
        #region Friend ids
        public IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userDTO, maxFriendsToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userId, maxFriendsToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userScreenName, maxFriendsToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFriendsToRetrieve);
        }
        #endregion

        #region Followers
        public IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userDTO, maxFollowersToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userId, maxFollowersToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userScreenName, maxFollowersToRetrieve);
            return ExecuteGetUserIdsQuery(query, maxFollowersToRetrieve);
        }
        #endregion

        #region Favourites
        public IEnumerable<ITweetDTO> GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userDTO, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        public IEnumerable<ITweetDTO> GetFavouriteTweets(long userId, int maxFavouritesToRetrieve)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userId, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        public IEnumerable<ITweetDTO> GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userScreenName, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }
        #endregion

        #region Block
        public bool BlockUser(IUserIdentifier userDTO)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool BlockUser(long userId)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool BlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }
        #endregion

        #region Stream Profile Image
        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL(userDTO, imageSize);
            return _webHelper.GetResponseStream(url);
        }
        #endregion
        // Helpers
        private IEnumerable<long> ExecuteGetUserIdsQuery(string query, int maxUserIds)
        {
            var userIdsDTO = _twitterAccessor.ExecuteCursorGETQuery<IIdsCursorQueryResultDTO>(query, maxUserIds);
            if (userIdsDTO == null)
            {
                return null;
            }

            var userIdsDTOList = userIdsDTO.ToList();

            var userdIds = new List<long>();
            for (int i = 0; i < userIdsDTOList.Count - 1; ++i)
            {
                userdIds.AddRange(userIdsDTOList.ElementAt(i).Ids);
            }

            // TODO : Move the limit logic in the TwitterAccessor.ExecuteCursorQuery

            if (userIdsDTOList.Any())
            {
                var userIdsDTOResult = userIdsDTOList.Last();
                userdIds.AddRange(userIdsDTOResult.Ids.Take(maxUserIds - userdIds.Count));
            }

            return userdIds;
        }
        #endregion

        #region async
        public async Task<IEnumerable<long>> GetFriendIdsAsync(IUserIdentifier userDTO, int maxFriendsToRetrieve)
        {

        }

        public async Task<IEnumerable<long>> GetFriendIdsAsync(long userId, int maxFriendsToRetrieve)
        {

        }

        public async Task<IEnumerable<long>> GetFriendIdsAsync(string userScreenName, int maxFriendsToRetrieve)
        {

        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(IUserIdentifier userDTO, int maxFollowersToRetrieve)
        {

        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(long userId, int maxFollowersToRetrieve)
        {

        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(string userScreenName, int maxFollowersToRetrieve)
        {

        }

        public async Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(IUserIdentifier userDTO, int maxFavouritesToRetrieve)
        {

        }

        public async Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(long userId, int maxFavouritesToRetrieve)
        {

        }

        public async Task<IEnumerable<ITweetDTO>> GetFavouriteTweetsAsync(string userScreenName, int maxFavouritesToRetrieve)
        {

        }

        public async Task<bool> BlockUserAsync(IUserIdentifier userDTO)
        {

        }

        public async Task<bool> BlockUserAsync(long userId)
        {

        }

        public async Task<bool> BlockUserAsync(string userScreenName)
        {

        }

        public async Task<Stream> GetProfileImageStreamAsync(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {

        }
        #endregion
    }
}