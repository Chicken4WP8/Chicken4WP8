﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Model;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class UserStream : TrackedStream, IUserStream
    {
        // TODO : access_revoked
        // TODO : Tweet tracking

        private readonly IMessageFactory _messageFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITweetListFactory _tweetListFactory;
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITaskFactory _taskFactory;

        public UserStream(
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            IMessageFactory messageFactory,
            IUserFactory userFactory,
            ITweetListFactory tweetListFactory,
            IJObjectStaticWrapper jObjectWrapper,
            IJsonObjectConverter jsonObjectConverter,
            IExceptionHandler exceptionHandler,
            ITwitterRequestGenerator twitterRequestGenerator,
            IStreamTrackManager<ITweet> streamTrackManager,
            ISynchronousInvoker synchronousInvoker,
            ITaskFactory taskFactory)
            : base(streamTrackManager, jsonObjectConverter, jObjectWrapper, streamResultGenerator, tweetFactory, twitterRequestGenerator, synchronousInvoker)
        {
            _messageFactory = messageFactory;
            _userFactory = userFactory;
            _tweetListFactory = tweetListFactory;
            _jObjectWrapper = jObjectWrapper;
            _exceptionHandler = exceptionHandler;
            _taskFactory = taskFactory;

            _events = new Dictionary<string, Action<JObject>>();

            InitializeEvents();
        }

        public event EventHandler StreamRunning;

        // Tweets
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByMe;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByFriend;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyone;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyoneButMe;

        // Messages
        public event EventHandler<MessageEventArgs> MessageSent;
        public event EventHandler<MessageEventArgs> MessageReceived;

        // Friends
        public event EventHandler<GenericEventArgs<IEnumerable<long>>> FriendIdsReceived;

        // Follow
        public event EventHandler<UserFollowedEventArgs> FollowedUser;
        public event EventHandler<UserFollowedEventArgs> FollowedByUser;

        public event EventHandler<UserFollowedEventArgs> UnFollowedUser;

        // Favourite
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByMe;
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyone;
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyoneButMe;

        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByMe;
        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyone;
        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyoneButMe;

        // List
        public event EventHandler<ListEventArgs> ListCreated;
        public event EventHandler<ListEventArgs> ListUpdated;
        public event EventHandler<ListEventArgs> ListDestroyed;

        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserAddedMemberToList;
        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserAddedToListBy;

        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserRemovedMemberFromList;
        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserRemovedFromListBy;

        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserSubscribedToListCreatedBy;
        public event EventHandler<ListUserUpdatedEventArgs> UserSubscribedToListCreatedByMe;

        public event EventHandler<ListUserUpdatedEventArgs> LoggedUserUnsubscribedToListCreatedBy;
        public event EventHandler<ListUserUpdatedEventArgs> UserUnsubscribedToListCreatedByMe;

        // Block
        public event EventHandler<UserBlockedEventArgs> BlockedUser;
        public event EventHandler<UserBlockedEventArgs> UnBlockedUser;

        // Profile Updated
        public event EventHandler<LoggedUserUpdatedEventArgs> LoggedUserProfileUpdated;

        // Warning
        public event EventHandler<WarningTooManyFollowersEventArgs> WarningTooManyFollowersDetected;

        private ILoggedUser _loggedUser;
        private HashSet<long> _friendIds;
        private readonly Dictionary<string, Action<JObject>> _events;

        private void InitializeEvents()
        {
            _events.Add("follow", TryRaiseUserFollowedEvent);
            _events.Add("favorite", TryRaiseFavouriteEvent);
            _events.Add("block", TryRaiseUserBlockedEvent);
            _events.Add("user_update", TryRaiseUserUpdatedEvent);

            _events.Add("unfollow", TryRaiseUserUnFollowedEvent);
            _events.Add("unfavorite", TryRaiseUnFavouriteEvent);
            _events.Add("unblock", TryRaiseUserUnBlockedEvent);

            // List events
            _events.Add("list_created", TryRaiseListCreatedEvent);
            _events.Add("list_updated", TryRaiseListUpdatedEvent);
            _events.Add("list_destroyed", TryRaiseListDestroyedEvent);
            _events.Add("list_member_added", TryRaiseListMemberAddedEvent);
            _events.Add("list_member_removed", TryRaiseListMemberRemovedEvent);
            _events.Add("list_member_subscribed", TryRaiseListMemberSubscribedEvent);
            _events.Add("list_member_unsubscribed", TryRaiseListMemberUnsubscribedEvent);
        }

        public void StartStream()
        {
            _synchronousInvoker.ExecuteSynchronously(StartStreamAsync());
        }

        public async Task StartStreamAsync()
        {
            _loggedUser = await _taskFactory.ExecuteTaskAsync(() => _userFactory.GetLoggedUser());
            if (_loggedUser == null)
            {
                StopStream(_exceptionHandler.LastExceptionInfos.WebException);
                return;
            }

            Func<HttpWebRequest> generateWebRequest = delegate
            {
                var query = Resources.Stream_UserStream;
                if (_filterLanguage != null)
                {
                    query = query.AddParameterToQuery("language", _filterLanguage);
                }

                return _twitterRequestGenerator.GetQueryWebRequest(query, HttpMethod.GET);
            };

            Action<string> eventReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                // We analyze the different types of message from the stream
                if (TryGetEvent(json)) return;
                if (TryGetTweet(json)) return;
                if (TryGetMessage(json)) return;
                if (TryGetWarning(json)) return;
                if (TryGetFriends(json)) return;

                TryInvokeGlobalStreamMessages(json);
            };

            await _streamResultGenerator.StartStreamAsync(eventReceived, generateWebRequest);
        }

        // Events
        private bool TryGetEvent(string jsonEvent)
        {
            var jsonObjectEvent = _jObjectWrapper.GetJobjectFromJson(jsonEvent);
            JToken jsonEventToken;

            if (jsonObjectEvent.TryGetValue("event", out jsonEventToken))
            {
                string eventName = jsonEventToken.Value<string>();

                if (_events.ContainsKey(eventName))
                {
                    _events[eventName].Invoke(jsonObjectEvent);
                }

                return true;
            }

            return false;
        }

        // Tweet
        private bool TryGetTweet(string jsonTweet)
        {
            try
            {
                var tweet = _tweetFactory.GenerateTweetFromJson(jsonTweet);
                if (tweet == null)
                {
                    return false;
                }

                var tweetReceivedEventArgs = new TweetReceivedEventArgs(tweet);
                this.Raise(TweetCreatedByAnyone, tweetReceivedEventArgs);

                if (tweet.Creator.Equals(_loggedUser))
                {
                    this.Raise(TweetCreatedByMe, tweetReceivedEventArgs);
                }
                else
                {
                    this.Raise(TweetCreatedByAnyoneButMe, tweetReceivedEventArgs);
                }

                if (_friendIds.Contains(tweet.Creator.Id))
                {
                    this.Raise(TweetCreatedByFriend, tweetReceivedEventArgs);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // Message
        private bool TryGetMessage(string jsonMessage)
        {
            var messageObject = _jObjectWrapper.GetJobjectFromJson(jsonMessage);
            JToken messageJToken;

            if (messageObject.TryGetValue("direct_message", out messageJToken))
            {
                var message = _messageFactory.GenerateMessageFromJson(messageJToken.ToString());
                if (message == null)
                {
                    return false;
                }

                var messageEventArgs = new MessageEventArgs(message);
                if (message.SenderId == _loggedUser.Id)
                {
                    this.Raise(MessageSent, messageEventArgs);
                }
                
                if (message.ReceiverId == _loggedUser.Id)
                {
                    this.Raise(MessageReceived, messageEventArgs);
                }

                return true;
            }

            return false;
        }

        // Follow
        private void TryRaiseUserFollowedEvent(JObject userFollowedEvent)
        {
            var source = GetSourceUser(userFollowedEvent);
            var target = GetTargetUser(userFollowedEvent);

            if (source.Equals(_loggedUser))
            {
                this.Raise(FollowedUser, new UserFollowedEventArgs(target));
            }
            else
            {
                this.Raise(FollowedByUser, new UserFollowedEventArgs(source));
            }
        }

        private void TryRaiseUserUnFollowedEvent(JObject userFollowedEvent)
        {
            var target = GetTargetUser(userFollowedEvent);
            this.Raise(UnFollowedUser, new UserFollowedEventArgs(target));
        }

        // Friends
        private bool TryGetFriends(string friendIdsJson)
        {
            JObject friendIdsObject = _jObjectWrapper.GetJobjectFromJson(friendIdsJson);
            JToken friendIdsToken;

            if (friendIdsObject.TryGetValue("friends", out friendIdsToken))
            {
                this.Raise(StreamRunning);

                var friendIds = friendIdsToken.Values<long>();
                _friendIds = new HashSet<long>(friendIds);
                this.Raise(FriendIdsReceived, new GenericEventArgs<IEnumerable<long>>(friendIds));

                return true;
            }

            return false;
        }

        // Favourite
        private void TryRaiseFavouriteEvent(JObject favouriteEvent)
        {
            var tweet = GetTweet(favouriteEvent);
            var source = GetSourceUser(favouriteEvent);

            var tweetFavouritedEventArgs = new TweetFavouritedEventArgs(tweet, source);

            this.Raise(TweetFavouritedByAnyone, tweetFavouritedEventArgs);

            if (source.Equals(_loggedUser))
            {
                this.Raise(TweetFavouritedByMe, tweetFavouritedEventArgs);
            }
            else
            {
                this.Raise(TweetFavouritedByAnyoneButMe, tweetFavouritedEventArgs);
            }
        }

        private void TryRaiseUnFavouriteEvent(JObject unFavouritedEvent)
        {
            var tweet = GetTweet(unFavouritedEvent);
            var source = GetSourceUser(unFavouritedEvent);

            var tweetFavouritedEventArgs = new TweetFavouritedEventArgs(tweet, source);

            this.Raise(TweetUnFavouritedByAnyone, tweetFavouritedEventArgs);

            if (source.Equals(_loggedUser))
            {
                this.Raise(TweetUnFavouritedByMe, tweetFavouritedEventArgs);
            }
            else
            {
                this.Raise(TweetUnFavouritedByAnyoneButMe, tweetFavouritedEventArgs);
            }
        }

        // List Created
        private void TryRaiseListCreatedEvent(JObject listCreatedEvent)
        {
            var list = GetList(listCreatedEvent);
            this.Raise(ListCreated, new ListEventArgs(list));
        }

        private void TryRaiseListUpdatedEvent(JObject listUpdatedEvent)
        {
            var list = GetList(listUpdatedEvent);
            this.Raise(ListUpdated, new ListEventArgs(list));
        }

        private void TryRaiseListDestroyedEvent(JObject listDestroyedEvent)
        {
            var list = GetList(listDestroyedEvent);
            this.Raise(ListDestroyed, new ListEventArgs(list));
        }

        private void TryRaiseListMemberAddedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_loggedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(LoggedUserAddedMemberToList, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(LoggedUserAddedToListBy, listEventArgs);
            }
        }

        private void TryRaiseListMemberRemovedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_loggedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(LoggedUserRemovedMemberFromList, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(LoggedUserRemovedFromListBy, listEventArgs);
            }
        }

        private void TryRaiseListMemberSubscribedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_loggedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(LoggedUserSubscribedToListCreatedBy, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(UserSubscribedToListCreatedByMe, listEventArgs);
            }
        }

        private void TryRaiseListMemberUnsubscribedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_loggedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(LoggedUserUnsubscribedToListCreatedBy, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(UserUnsubscribedToListCreatedByMe, listEventArgs);
            }
        }

        // Block
        private void TryRaiseUserBlockedEvent(JObject userBlockedEvent)
        {
            var target = GetTargetUser(userBlockedEvent);
            this.Raise(BlockedUser, new UserBlockedEventArgs(target));
        }

        private void TryRaiseUserUnBlockedEvent(JObject userUnBlockedEvent)
        {
            var target = GetTargetUser(userUnBlockedEvent);
            this.Raise(UnBlockedUser, new UserBlockedEventArgs(target));
        }

        // User Update
        private void TryRaiseUserUpdatedEvent(JObject userUpdatedEvent)
        {
            var source = GetSourceUser(userUpdatedEvent);
            var newLoggedUser = _userFactory.GenerateLoggedUserFromDTO(source.UserDTO);

            this.Raise(LoggedUserProfileUpdated, new LoggedUserUpdatedEventArgs(newLoggedUser));
        }

        // Warnings
        private bool TryGetWarning(string warningJson)
        {
            var jsonObjectWarning = _jObjectWrapper.GetJobjectFromJson(warningJson);
            JToken jsonWarning;

            if (jsonObjectWarning.TryGetValue("warning", out jsonWarning))
            {
                return TryRaiseTooMuchFollowerWarning(jsonWarning);
            }

            return false;
        }

        private bool TryRaiseTooMuchFollowerWarning(JToken jsonWarning)
        {
            if (jsonWarning["user_id"] != null)
            {
                var warningMessage = _jsonObjectConverter.DeserializeObject<WarningMessageTooManyFollowers>(jsonWarning.ToString());
                this.Raise(WarningTooManyFollowersDetected, new WarningTooManyFollowersEventArgs(warningMessage));
                return true;
            }

            return false;
        }

        #region Get Json Info

        private IUser GetSourceUser(JObject eventInfo)
        {
            var jsonSource = eventInfo["source"].ToString();
            return _userFactory.GenerateUserFromJson(jsonSource);
        }

        private IUser GetTargetUser(JObject eventInfo)
        {
            var jsonTarget = eventInfo["target"].ToString();
            return _userFactory.GenerateUserFromJson(jsonTarget);
        }

        private ITweetList GetList(JObject listEvent)
        {
            var jsonList = listEvent["target_object"].ToString();
            return _tweetListFactory.GenerateTweetListFromJson(jsonList);
        }

        private ITweet GetTweet(JObject tweetEvent)
        {
            var jsonTweet = tweetEvent["target_object"].ToString();
            return _tweetFactory.GenerateTweetFromJson(jsonTweet);
        }

        #endregion
    }
}