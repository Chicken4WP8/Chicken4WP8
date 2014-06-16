using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// A token user is unique to a Token and provides action that will
    /// be executed from the connected user and that are not available
    /// from another user like (read my messages)
    /// </summary>
    public class LoggedUser : User, ILoggedUser
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITweetController _tweetController;
        private readonly ITweetFactory _tweetFactory;
        private readonly IMessageController _messageController;
        private readonly IFriendshipController _friendshipController;
        private readonly IAccountController _accountController;
        private readonly ISavedSearchController _savedSearchController;

        private IOAuthCredentials _savedCredentials;

        public LoggedUser(
            IUserDTO userDTO,
            ICredentialsAccessor credentialsAccessor,
            ITimelineController timelineController,
            ITweetController tweetController,
            ITweetFactory tweetFactory,
            IUserController userController,
            IMessageController messageController,
            IFriendshipController friendshipController,
            IAccountController accountController,
            ISavedSearchController savedSearchController,
            ITaskFactory taskFactory)

            : base(userDTO, timelineController, userController, friendshipController, taskFactory)
        {
            _credentialsAccessor = credentialsAccessor;
            _tweetController = tweetController;
            _tweetFactory = tweetFactory;
            _messageController = messageController;
            _friendshipController = friendshipController;
            _accountController = accountController;
            _savedSearchController = savedSearchController;

            Credentials = _credentialsAccessor.CurrentThreadCredentials;
        }

        public void SetCredentials(IOAuthCredentials credentials)
        {
            Credentials = credentials;
        }

        public IOAuthCredentials Credentials { get; private set; }
        public IEnumerable<IMessage> LatestDirectMessagesReceived { get; set; }
        public IEnumerable<IMessage> LatestDirectMessagesSent { get; set; }
        public IEnumerable<IMention> LatestMentionsTimeline { get; set; }
        public IEnumerable<ITweet> LatestHomeTimeline { get; set; }
        public IEnumerable<ISuggestedUserList> SuggestedUserList { get; set; }
        public IEnumerable<IUser> BlockedUsers { get; set; }
        public IEnumerable<long> BlockedUsersIds { get; set; }

        private void StartLoggedUserOperation()
        {
            _savedCredentials = _credentialsAccessor.CurrentThreadCredentials;
            _credentialsAccessor.CurrentThreadCredentials = Credentials;
        }

        private void CompletedLoggedUserOperation()
        {
            _credentialsAccessor.CurrentThreadCredentials = _savedCredentials;
        }

        private T StartLoggedUserOperation<T>(Func<T> operation)
        {
            StartLoggedUserOperation();
            var result = operation();
            CompletedLoggedUserOperation();
            return result;
        }

        private void StartLoggedUserOperation(Action operation)
        {
            StartLoggedUserOperation();
            operation();
            CompletedLoggedUserOperation();
        }

        // Home Timeline
        public IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweets = 40)
        {
            return StartLoggedUserOperation(() => _timelineController.GetHomeTimeline(maximumNumberOfTweets));
        }

        public IEnumerable<ITweet> GetHomeTimeline(ITimelineRequestParameters timelineRequestParameters)
        {
            return StartLoggedUserOperation(() => _timelineController.GetHomeTimeline((IHomeTimelineRequestParameters) timelineRequestParameters));
        }

        // Mentions Timeline
        public IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfMentions = 40)
        {
            return StartLoggedUserOperation(() => _timelineController.GetMentionsTimeline(maximumNumberOfMentions));
        }

        // Frienships
        public IEnumerable<IRelationshipState> GetRelationshipStatesWith(IEnumerable<IUser> users)
        {
            return StartLoggedUserOperation(() => _friendshipController.GetRelationshipStatesWith(users));
        }

        public Dictionary<IUser, IRelationshipState> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> users)
        {
            return StartLoggedUserOperation(() => _friendshipController.GetRelationshipStatesAssociatedWith(users));
        }

        // Friends - Followers
        public IEnumerable<IUser> GetUsersRequestingFriendship()
        {
            return StartLoggedUserOperation(() => _friendshipController.GetUsersRequestingFriendship());
        }

        public IEnumerable<IUser> GetUsersYouRequestedToFollow()
        {
            return StartLoggedUserOperation(() => _friendshipController.GetUsersYouRequestedToFollow());
        }

        // Follow
        public bool FollowUser(IUser user)
        {
            return StartLoggedUserOperation(() => _friendshipController.CreateFriendshipWith(user));
        }

        public bool FollowUser(long userId)
        {
            return StartLoggedUserOperation(() => _friendshipController.CreateFriendshipWith(userId));
        }

        public bool UnFollowUser(IUser user)
        {
            return StartLoggedUserOperation(() => _friendshipController.DestroyFriendshipWith(user));
        }

        public bool UnFollowUser(long userId)
        {
            return StartLoggedUserOperation(() => _friendshipController.DestroyFriendshipWith(userId));
        }

        public bool UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return StartLoggedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotificationsEnabled));
        }

        public IEnumerable<ISavedSearch> GetSavedSearches()
        {
            return StartLoggedUserOperation(() => _savedSearchController.GetSavedSearches());
        }

        // Direct Messages
        public IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = 40)
        {
            return StartLoggedUserOperation(() => _messageController.GetLatestMessagesReceived(maximumMessages));
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = 40)
        {
            return StartLoggedUserOperation(() => _messageController.GetLatestMessagesSent(maximumMessages));
        }

        public IMessage PublishMessage(IMessage message)
        {
            return StartLoggedUserOperation(() => _messageController.PublishMessage(message.MessageDTO));
        }

        // Tweet
        public ITweet PublishTweet(string text)
        {
            var tweet = _tweetFactory.CreateTweet(text);
            PublishTweet(tweet);
            return tweet;
        }

        public bool PublishTweet(ITweet tweet)
        {
            _tweetController.PublishTweet(tweet);
            StartLoggedUserOperation(() => _tweetController.PublishTweet(tweet));
            return tweet.IsTweetPublished;
        }

        // Get Blocked Users
        public IEnumerable<IUser> GetBlockedUsers(bool createBlockUsers = true, bool createBlockedUsersIds = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<long> GetBlockedUsersIds(bool createBlockedUsersIds = true)
        {
            throw new NotImplementedException();
        }

        // Get Suggested List
        public IEnumerable<ISuggestedUserList> GetSuggestedUserList(bool createSuggestedUserList = true)
        {
            throw new NotImplementedException();
        }

        public IAccountSettings AccountSettings { get; set; }

        /// <summary>
        /// Retrieve the settings of the Token's owner
        /// </summary>
        public IAccountSettings GetAccountSettings()
        {
            return StartLoggedUserOperation(() => _accountController.GetLoggedUserSettings());
        }

        #region Async
        
        public async Task<IEnumerable<IMessage>> GetLatestMessagesReceivedAsync(int count = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetLatestMessagesReceived(count));
        }

        public async Task<IEnumerable<IMessage>> GetLatestMessagesSentAsync(int maximumMessages = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetLatestMessagesSent(maximumMessages));
        }

        public async Task<IMessage> PublishMessageAsync(IMessage message)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishMessage(message));
        }

        // Home Timeline
        public async Task<IEnumerable<ITweet>> GetHomeTimelineAsync(int count = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetHomeTimeline(count));
        }

        public async Task<IEnumerable<ITweet>> GetHomeTimelineAsync(ITimelineRequestParameters timelineRequestParameters)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetHomeTimeline(timelineRequestParameters));
        }

        // Mentions Timeline
        public async Task<IEnumerable<IMention>> GetMentionsTimelineAsync(int count = 40)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetMentionsTimeline(count));
        }

        public async Task<IEnumerable<IRelationshipState>> GetRelationshipStatesWithAsync(IEnumerable<IUser> users)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetRelationshipStatesWith(users));
        }

        public async Task<Dictionary<IUser, IRelationshipState>> GetRelationshipStatesAssociatedWithAsync(IEnumerable<IUser> users)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetRelationshipStatesAssociatedWith(users));
        }

        public async Task<IEnumerable<IUser>> GetUsersRequestingFriendshipAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetUsersRequestingFriendship());
        }

        public async Task<IEnumerable<IUser>> GetUsersYouRequestedToFollowAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetUsersYouRequestedToFollow());
        }

        public async Task<bool> FollowUserAsync(IUser user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => FollowUser(user));
        }

        public async Task<bool> UnFollowUserAsync(IUser user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => UnFollowUser(user));
        }

        public async Task<bool> UpdateRelationshipAuthorizationsWithAsync(IUser user, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return await _taskFactory.ExecuteTaskAsync(() => UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotificationsEnabled));
        }

        public async Task<IEnumerable<ISavedSearch>> GetSavedSearchesAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetSavedSearches());
        }

        public async Task<IEnumerable<IUser>> GetBlockedUsersAsync(bool createBlockUsers = true, bool createBlockedUsersIds = true)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetBlockedUsers(createBlockUsers, createBlockedUsersIds));
        }

        public async Task<IEnumerable<long>> GetBlockedUsersIdsAsync(bool createBlockedUsersIds = true)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetBlockedUsersIds(createBlockedUsersIds));
        }

        public async Task<IEnumerable<ISuggestedUserList>> GetSuggestedUserListAsync(bool createSuggestedUserList = true)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetSuggestedUserList(createSuggestedUserList));
        }

        public async Task<IAccountSettings> GetAccountSettingsAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetAccountSettings());
        } 
        #endregion
    }
}