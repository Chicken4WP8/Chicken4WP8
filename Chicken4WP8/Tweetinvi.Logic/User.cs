﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// Provide information and functions that a user can do
    /// </summary>
    public class User : IUser
    {
        protected IUserDTO _userDTO;
        protected readonly ITimelineController _timelineController;
        protected readonly IUserController _userController;
        private readonly IFriendshipController _friendshipController;
        protected readonly ITaskFactory _taskFactory;

        public IUserDTO UserDTO
        {
            get { return _userDTO; }
            set { _userDTO = value; }
        }

        public IUserIdentifier UserIdentifier
        {
            get { return _userDTO; }
        }

        #region Public Attributes

        #region Twitter API Attributes

        // This region represents the information accessible from a Twitter API
        // when querying for a User

        public long Id
        {
            get { return _userDTO.Id; }
        }

        public string IdStr
        {
            get { return _userDTO.IdStr; }
        }

        public string Name
        {
            get { return _userDTO.Name; }
        }

        public string ScreenName
        {
            get { return _userDTO.ScreenName; }
        }

        public string Description
        {
            get { return _userDTO.Description; }
        }

        public ITweetDTO Status
        {
            get { return _userDTO.Status; }
        }

        public DateTime CreatedAt
        {
            get { return _userDTO.CreatedAt; }
        }

        public string Location
        {
            get { return _userDTO.Location; }
        }

        public bool GeoEnabled
        {
            get { return _userDTO.GeoEnabled; }
        }

        public string Url
        {
            get { return _userDTO.Url; }
        }

        public Language Language
        {
            get { return _userDTO.Language; }
        }

        public int StatusesCount
        {
            get { return _userDTO.StatusesCount; }
        }

        public int FollowersCount
        {
            get { return _userDTO.FollowersCount; }
        }

        public int FriendsCount
        {
            get { return _userDTO.FriendsCount; }
        }

        public bool Following
        {
            get { return _userDTO.Following; }
        }

        public bool Protected
        {
            get { return _userDTO.Protected; }
        }

        public bool Verified
        {
            get { return _userDTO.Verified; }
        }

        public IUserEntities Entities
        {
            get { return _userDTO.Entities; }
        }

        public string ProfileImageUrl
        {
            get { return _userDTO.ProfileImageUrl; }
        }

        public string ProfileImageUrlHttps
        {
            get { return _userDTO.ProfileImageUrlHttps; }
        }

        public bool FollowRequestSent
        {
            get { return _userDTO.FollowRequestSent; }
        }

        public bool DefaultProfile
        {
            get { return _userDTO.DefaultProfile; }
        }

        public bool DefaultProfileImage
        {
            get { return _userDTO.DefaultProfileImage; }
        }

        public int FavouritesCount
        {
            get { return _userDTO.FavouritesCount ?? 0; }
        }

        public int ListedCount
        {
            get { return _userDTO.ListedCount ?? 0; }
        }

        public string ProfileSidebarFillColor
        {
            get { return _userDTO.ProfileSidebarFillColor; }
        }

        public string ProfileSidebarBorderColor
        {
            get { return _userDTO.ProfileSidebarBorderColor; }
        }

        public bool ProfileBackgroundTitle
        {
            get { return _userDTO.ProfileBackgroundTile; }
        }

        public string ProfileBackgroundColor
        {
            get { return _userDTO.ProfileBackgroundColor; }
        }

        public string ProfileBackgroundImageUrl
        {
            get { return _userDTO.ProfileBackgroundImageUrl; }
        }

        public string ProfileBackgroundImageUrlHttps
        {
            get { return _userDTO.ProfileBackgroundImageUrlHttps; }
        }

        public string ProfileBannerURL
        {
            get { return _userDTO.ProfileBannerURL; }
        }

        public string ProfileTextColor
        {
            get { return _userDTO.ProfileTextColor; }
        }

        public string ProfileLinkColor
        {
            get { return _userDTO.ProfileLinkColor; }
        }

        public bool ProfileUseBackgroundImage
        {
            get { return _userDTO.ProfileUseBackgroundImage; }
        }

        public bool IsTranslator
        {
            get { return _userDTO.IsTranslator; }
        }

        public bool ShowAllInlineMedia
        {
            get { return _userDTO.ShowAllInlineMedia; }
        }

        public bool ContributorsEnabled
        {
            get { return _userDTO.ContributorsEnabled; }
        }

        public int? UtcOffset
        {
            get { return _userDTO.UtcOffset; }
        }

        public string TimeZone
        {
            get { return _userDTO.TimeZone; }
        }

        public IEnumerable<string> WithheldInCountries
        {
            get { return _userDTO.WithheldInCountries; }
        }

        public string WithheldScope
        {
            get { return _userDTO.WithheldScope; }
        }

        [Obsolete("Twitter's documentation states that this property is deprecated")]
        public bool Notifications
        {
            get { return _userDTO.Notifications; }
        }

        #endregion

        #region Tweetinvi API Attributes

        public List<long> FriendIds { get; set; }
        public List<IUser> Friends { get; set; }
        public List<long> FollowerIds { get; set; }
        public List<IUser> Followers { get; set; }
        public List<IUser> Contributors { get; set; }
        public List<IUser> Contributees { get; set; }
        public List<ITweet> Timeline { get; set; }
        public List<ITweet> Retweets { get; set; }
        public List<ITweet> FriendsRetweets { get; set; }
        public List<ITweet> TweetsRetweetedByFollowers { get; set; }

        #endregion

        #endregion

        public User(
            IUserDTO userDTO,
            ITimelineController timelineController,
            IUserController userController,
            IFriendshipController friendshipController,
            ITaskFactory taskFactory)
        {
            _userDTO = userDTO;
            _timelineController = timelineController;
            _userController = userController;
            _friendshipController = friendshipController;
            _taskFactory = taskFactory;
        }
        #region sync
        // Friends
        public virtual IEnumerable<long> GetFriendIds(int maxFriendsToRetrieve = 5000)
        {
            return _userController.GetFriendIds(_userDTO, maxFriendsToRetrieve);
        }

        public virtual IEnumerable<IUser> GetFriends(int maxFriendsToRetrieve = 250)
        {
            return _userController.GetFriends(_userDTO, maxFriendsToRetrieve);
        }

        // Followers
        public virtual IEnumerable<long> GetFollowerIds(int maxFriendsToRetrieve = 5000)
        {
            return _userController.GetFollowerIds(_userDTO, maxFriendsToRetrieve);
        }

        public virtual IEnumerable<IUser> GetFollowers(int maxFriendsToRetrieve = 250)
        {
            return _userController.GetFollowers(_userDTO, maxFriendsToRetrieve);
        }

        // Relationship
        public virtual IRelationship GetRelationshipWith(IUser targetUser)
        {
            if (targetUser == null)
            {
                return null;
            }

            return _friendshipController.GetRelationshipBetween(_userDTO, targetUser.UserDTO);
        }

        // Timeline
        public IEnumerable<ITweet> GetUserTimeline(int maximumNumberOfTweets = 40)
        {
            return _timelineController.GetUserTimeline(_userDTO, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(IUserTimelineRequestParameters timelineRequestParameters)
        {
            return _timelineController.GetUserTimeline(timelineRequestParameters);
        }

        // Favorites
        public virtual IEnumerable<ITweet> GetFavorites(int maximumNumberOfTweets = 40)
        {
            return _userController.GetFavouriteTweets(_userDTO, maximumNumberOfTweets);
        }

        // Block User
        public virtual bool Block()
        {
            return _userController.BlockUser(_userDTO);
        }

        // Stream Profile IMage
        public Stream GetProfileImageStream(ImageSize imageSize = ImageSize.normal)
        {
            return _userController.GetProfileImageStream(_userDTO, imageSize);
        }

        // Contributors
        public IEnumerable<IUser> GetContributors(bool createContributorList = false)
        {
            // string query = Resources.User_GetContributors;
            throw new NotImplementedException();
        }

        // Contributees
        public IEnumerable<IUser> GetContributees(bool createContributeeList = false)
        {
            // string query = Resources.User_GetContributees;
            throw new NotImplementedException();
        }
        #endregion

        #region Async
        public async Task<IEnumerable<long>> GetFriendIdsAsync(int maxFriendsToRetrieve = 5000)
        {
            return await _userController.GetFriendIdsAsync(maxFriendsToRetrieve);
        }

        public async Task<IEnumerable<IUser>> GetFriendsAsync(int maxFriendsToRetrieve = 250)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetFriends(maxFriendsToRetrieve));
        }

        public async Task<IEnumerable<long>> GetFollowerIdsAsync(int maxFriendsToRetrieve = 5000)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetFollowerIds(maxFriendsToRetrieve));
        }

        public async Task<IEnumerable<IUser>> GetFollowersAsync(int maxFriendsToRetrieve = 250)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetFollowers(maxFriendsToRetrieve));
        }

        public async Task<IRelationship> GetRelationshipWithAsync(IUser user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetRelationshipWith(user));
        }

        public async Task<IEnumerable<ITweet>> GetUserTimelineAsync(int maximumTweet = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetUserTimeline(maximumTweet));
        }

        public async Task<IEnumerable<ITweet>> GetUserTimelineAsync(IUserTimelineRequestParameters timelineRequestParameters)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetUserTimeline(timelineRequestParameters));
        }

        public async Task<IEnumerable<ITweet>> GetFavoritesAsync(int maximumTweets = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetFavorites(maximumTweets));
        }

        public async Task<bool> BlockAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Block());
        }

        public async Task<Stream> GetProfileImageStreamAsync(ImageSize imageSize = ImageSize.normal)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetProfileImageStream(imageSize));
        }

        public async Task<IEnumerable<IUser>> GetContributorsAsync(bool createContributorList = false)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetContributors(createContributorList));
        }

        public async Task<IEnumerable<IUser>> GetContributeesAsync(bool createContributeeList = false)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetContributees(createContributeeList));
        }

        #endregion

        public bool Equals(IUser other)
        {
            return Id == other.Id || ScreenName == other.ScreenName;
        }

        public override string ToString()
        {
            return _userDTO != null ? _userDTO.Name : "Undefined";
        }
    }
}