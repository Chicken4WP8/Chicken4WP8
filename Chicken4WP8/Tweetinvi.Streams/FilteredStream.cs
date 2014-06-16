﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.Model;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class FilteredStream : TrackedStream, IFilteredStream
    {
        // Const
        private const int MAXIMUM_TRACKED_LOCATIONS_AUTHORIZED = 25;
        private const int MAXIMUM_TRACKED_USER_ID_AUTHORIZED = 5000;

        // Events
        public event EventHandler<MatchedTweetAndLocationReceivedEventArgs> MatchingTweetAndLocationReceived;

        // Properties
        private readonly Dictionary<long?, Action<ITweet>> _followingUserIds;
        public Dictionary<long?, Action<ITweet>> FollowingUserIds
        {
            get { return _followingUserIds; }
        }

        private readonly Dictionary<ILocation, Action<ITweet>> _locations;
        public Dictionary<ILocation, Action<ITweet>> Locations
        {
            get { return _locations; }
        }

        // Constructor
        public FilteredStream(
            IStreamTrackManager<ITweet> streamTrackManager,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            ITwitterRequestGenerator twitterRequestGenerator,
            ISynchronousInvoker synchronousInvoker)

            : base(
                streamTrackManager,
                jsonObjectConverter,
                jObjectStaticWrapper,
                streamResultGenerator,
                tweetFactory,
                twitterRequestGenerator, 
                synchronousInvoker)
        {
            _followingUserIds = new Dictionary<long?, Action<ITweet>>();
            _locations = new Dictionary<ILocation, Action<ITweet>>();
        }

        public void StartStreamMatchingAnyCondition()
        {
            _synchronousInvoker.ExecuteSynchronously(StartStreamMatchingAnyConditionAsync());
        }

        public async Task StartStreamMatchingAnyConditionAsync()
        {
            Func<HttpWebRequest> generateWebRequest = () =>
            {
                var query = GenerateORFilterQuery();

                if (_filterLanguage != null)
                {
                    query = query.AddParameterToQuery("language", _filterLanguage);
                }

                return _twitterRequestGenerator.GetQueryWebRequest(query, HttpMethod.POST);
            };

            Action<string> tweetReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var matchingTrackAndActions = _streamTrackManager.GetMatchingTracksAndActions(tweet.Text);
                var matchingTracks = matchingTrackAndActions.Select(x => x.Item1);
                var machingLocationAndActions = GetMatchedLocations(tweet.Coordinates);
                var matchingLocations = machingLocationAndActions.Select(x => x.Key);

                CallMultipleActions(tweet, matchingTrackAndActions.Select(x => x.Item2));
                CallMultipleActions(tweet, machingLocationAndActions.Select(x => x.Value));
                CallFollowerAction(tweet);

                RaiseMatchingTweetReceived(new MatchedTweetReceivedEventArgs(tweet, matchingTracks));
                this.Raise(MatchingTweetAndLocationReceived, new MatchedTweetAndLocationReceivedEventArgs(tweet, matchingTracks, matchingLocations));
            };

            await _streamResultGenerator.StartStreamAsync(tweetReceived, generateWebRequest);
        }

        public void StartStreamMatchingAllConditions()
        {
            _synchronousInvoker.ExecuteSynchronously(StartStreamMatchingAllConditionsAsync());
        }

        public async Task StartStreamMatchingAllConditionsAsync()
        {
            Func<HttpWebRequest> generateWebRequest = () =>
            {
                var query = GenerateANDFilterQuery();

                if (_filterLanguage != null)
                {
                    query = query.AddParameterToQuery("language", _filterLanguage);
                }

                return _twitterRequestGenerator.GetQueryWebRequest(query, HttpMethod.POST);
            };

            Action<string> tweetReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var matchingTrackAndActions = _streamTrackManager.GetMatchingTracksAndActions(tweet.Text);
                var matchingTracks = matchingTrackAndActions.Select(x => x.Item1);
                var machingLocationAndActions = GetMatchedLocations(tweet.Coordinates);
                var matchingLocations = machingLocationAndActions.Select(x => x.Key);

                if (!DoestTheTweetMatchAllConditions(tweet, matchingTracks, matchingLocations))
                {
                    return;
                }

                CallMultipleActions(tweet, matchingTrackAndActions.Select(x => x.Item2));
                CallMultipleActions(tweet, machingLocationAndActions.Select(x => x.Value));
                CallFollowerAction(tweet);

                RaiseMatchingTweetReceived(new MatchedTweetReceivedEventArgs(tweet, matchingTracks));
                this.Raise(MatchingTweetAndLocationReceived, new MatchedTweetAndLocationReceivedEventArgs(tweet, matchingTracks, matchingLocations));
            };

            await _streamResultGenerator.StartStreamAsync(tweetReceived, generateWebRequest);
        }

        private void CallMultipleActions<T>(T tweet, IEnumerable<Action<T>> tracksActionsIdenfied)
        {
            if (tracksActionsIdenfied != null)
            {
                tracksActionsIdenfied.Foreach(action =>
                {
                    if (action != null)
                    {
                        action(tweet);
                    }
                });
            }
        }

        private void CallFollowerAction(ITweet tweet)
        {
            var isFollowerTracked = ContainsFollow(tweet.Creator);
            if (isFollowerTracked && _followingUserIds[tweet.Creator.Id] != null)
            {
                _followingUserIds[tweet.Creator.Id](tweet);
            }
        }

        private bool DoestTheTweetMatchAllConditions(ITweet tweet, IEnumerable<string> matchingTracks, IEnumerable<ILocation> matchingLocations)
        {
            if (tweet.Creator.Id == TweetinviConfig.DEFAULT_ID)
            {
                return false;
            }

            bool followMatches = FollowingUserIds.IsEmpty() || ContainsFollow(tweet.Creator.Id);
            bool tracksMatches = Tracks.IsEmpty() || matchingTracks.Any();
            bool locationMatches = Locations.IsEmpty() || matchingLocations.Any();

            if (FollowingUserIds.Any())
            {
                return followMatches && tracksMatches && locationMatches;
            }
            
            if (Tracks.Any())
            {
                return tracksMatches && locationMatches;
            }
            
            if (Locations.Any())
            {
                return locationMatches;
            }

            return true;
        }

        #region Generate Query

        private string GenerateORFilterQuery()
        {
            StringBuilder queryBuilder = new StringBuilder(Resources.Stream_Filter);

            var followPostRequest = QueryGeneratorHelper.GenerateFilterFollowRequest(FollowingUserIds.Keys.ToList());
            var trackPostRequest = QueryGeneratorHelper.GenerateFilterTrackRequest(Tracks.Keys.ToList());
            var locationPostRequest = QueryGeneratorHelper.GenerateFilterLocationRequest(Locations.Keys.ToList());

            if (!String.IsNullOrEmpty(trackPostRequest))
            {
                queryBuilder.Append(trackPostRequest);
            }

            if (!String.IsNullOrEmpty(followPostRequest))
            {
                queryBuilder.Append(queryBuilder.Length == 0 ? followPostRequest : String.Format("&{0}", followPostRequest));
            }

            if (!String.IsNullOrEmpty(locationPostRequest))
            {
                queryBuilder.Append(queryBuilder.Length == 0 ? locationPostRequest : String.Format("&{0}", locationPostRequest));
            }

            return queryBuilder.ToString();
        }

        private string GenerateANDFilterQuery()
        {
            StringBuilder queryBuilder = new StringBuilder(Resources.Stream_Filter);

            var followPostRequest = QueryGeneratorHelper.GenerateFilterFollowRequest(FollowingUserIds.Keys.ToList());
            var trackPostRequest = QueryGeneratorHelper.GenerateFilterTrackRequest(Tracks.Keys.ToList());
            var locationPostRequest = QueryGeneratorHelper.GenerateFilterLocationRequest(Locations.Keys.ToList());

            if (!String.IsNullOrEmpty(followPostRequest))
            {
                queryBuilder.Append(followPostRequest);
            }
            else if (!String.IsNullOrEmpty(trackPostRequest))
            {
                queryBuilder.Append(trackPostRequest);
            }
            else if (!String.IsNullOrEmpty(locationPostRequest))
            {
                queryBuilder.Append(locationPostRequest);
            }

            return queryBuilder.ToString();
        }

        #endregion

        #region Follow

        public void AddFollow(long? userId, Action<ITweet> userPublishedTweet = null)
        {
            if (userId != null && _followingUserIds.Count < MAXIMUM_TRACKED_USER_ID_AUTHORIZED)
            {
                _followingUserIds.Add(userId, userPublishedTweet);
            }
        }

        public void AddFollow(IUser user, Action<ITweet> userPublishedTweet = null)
        {
            if (user != null && user.Id != TweetinviConfig.DEFAULT_ID)
            {
                AddFollow(user.Id, userPublishedTweet);
            }
        }

        public void RemoveFollow(long? userId)
        {
            if (userId != null)
            {
                _followingUserIds.Remove(userId);
            }
        }

        public void RemoveFollow(IUser user)
        {
            if (user != null)
            {
                RemoveFollow(user.Id);
            }
        }

        public bool ContainsFollow(long? userId)
        {
            if (userId != null)
            {
                return _followingUserIds.Keys.Contains(userId);
            }

            return false;
        }

        public bool ContainsFollow(IUser user)
        {
            if (user != null)
            {
                return ContainsFollow(user.Id);
            }

            return false;
        }

        public void ClearFollows()
        {
            _followingUserIds.Clear();
        }

        #endregion

        #region Location

        public ILocation AddLocation(ICoordinates coordinate1, ICoordinates coordinate2, Action<ITweet> locationDetected = null)
        {
            ILocation location = new Location(coordinate1, coordinate2);
            AddLocation(location, locationDetected);

            return location;
        }

        public void AddLocation(ILocation location, Action<ITweet> locationDetected = null)
        {
            if (!ContainsLocation(location) && _locations.Count < MAXIMUM_TRACKED_LOCATIONS_AUTHORIZED)
            {
                Locations.Add(location, locationDetected);
            }
        }

        public void RemoveLocation(ICoordinates coordinate1, ICoordinates coordinate2)
        {
            var location = Locations.Keys.FirstOrDefault(x => (x.Coordinate1 == coordinate1 && x.Coordinate2 == coordinate2) ||
                                                              (x.Coordinate1 == coordinate2 && x.Coordinate2 == coordinate1));

            if (location != null)
            {
                Locations.Remove(location);
            }
        }

        public void RemoveLocation(ILocation location)
        {
            RemoveLocation(location.Coordinate1, location.Coordinate2);
        }

        public bool ContainsLocation(ICoordinates coordinate1, ICoordinates coordinate2)
        {
            return Locations.Keys.Any(x => (x.Coordinate1 == coordinate1 && x.Coordinate2 == coordinate2) ||
                                           (x.Coordinate1 == coordinate2 && x.Coordinate2 == coordinate1));
        }

        public bool ContainsLocation(ILocation location)
        {
            return ContainsLocation(location.Coordinate1, location.Coordinate2);
        }

        public void ClearLocations()
        {
            Locations.Clear();
        }

        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(ICoordinates coordinates)
        {
            if (_locations.IsEmpty() || coordinates == null)
            {
                return new List<KeyValuePair<ILocation, Action<ITweet>>>();
            }

            return _locations.Where(x => Location.CoordinatesLocatedIn(coordinates, x.Key)).ToList();
        }

        #endregion
    }
}