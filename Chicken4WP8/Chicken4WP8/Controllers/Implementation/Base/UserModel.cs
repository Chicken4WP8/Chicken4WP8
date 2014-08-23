using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CoreTweet;
using Newtonsoft.Json;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class UserModel : PropertyChangedBase, IUserModel
    {
        public UserModel()
        { }

        public UserModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            ScreenName = user.ScreenName;
            CreatedAt = user.CreatedAt.LocalDateTime;
            Description = user.Description;
            IsVerified = user.IsVerified;
            IsPrivate = user.IsProtected;
            IsTranslator = user.IsTranslator;
            Location = user.Location;
            TweetsCount = user.StatusesCount;
            FollowingCount = user.FriendsCount;
            FollowersCount = user.FollowersCount;
            FavoritesCount = user.FavouritesCount;
            if (user.ProfileImageUrl != null)
                ProfileImageUrl = user.ProfileImageUrl.AbsoluteUri.Replace("_normal", "_bigger");
            if (user.Url != null)
                Url = user.Url.AbsoluteUri;
            if (user.ProfileBannerUrl != null)
                UserProfileBannerImageUrl = user.ProfileBannerUrl.AbsoluteUri + "/ipad_retina";
            if (user.Entities != null)
                Entities = new UserEntititesModel(user.Entities);
        }

        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsTranslator { get; set; }
        /// <summary>
        /// default, use bigger avatar
        /// </summary>
        public string ProfileImageUrl { get; set; }
        public string UserProfileBannerImageUrl { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public IUserEntities Entities { get; set; }
        public int TweetsCount { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
        public int FavoritesCount { get; set; }
        #region for template
        private bool isFollowing;
        public bool IsFollowing
        {
            get { return isFollowing; }
            set
            {
                isFollowing = value;
                NotifyOfPropertyChange(() => IsFollowing);
            }
        }
        private bool isFollowedBy;
        public bool IsFollowedBy
        {
            get { return isFollowedBy; }
            set
            {
                isFollowedBy = value;
                NotifyOfPropertyChange(() => IsFollowedBy);
            }
        }
        private List<IEntity> parsedEntities;
        [JsonIgnore]
        public List<IEntity> ParsedEntities
        {
            get
            {
                if (string.IsNullOrEmpty(Description)) return null;
                if (parsedEntities != null) return parsedEntities;
                parsedEntities = new List<IEntity>();
                parsedEntities.AddRange(Utils.ParseUserMentions(Description));
                parsedEntities.AddRange(Utils.ParseHashTags(Description));
                if (Entities != null &&
                    Entities.Description != null &&
                    Entities.Description.Urls != null)
                {
                    var parsedUrls = Utils.ParseUrls(Description, Entities.Description.Urls);
                    parsedEntities.AddRange(parsedUrls);
                }
                return parsedEntities;
            }
        }
        private bool isProfileDetail;
        public bool IsProfileDetail
        {
            get { return isProfileDetail; }
            set
            {
                isProfileDetail = value;
                NotifyOfPropertyChange(() => IsProfileDetail);
            }
        }
        private byte[] profileImageData;
        [JsonIgnore]
        public byte[] ProfileImageData
        {
            get { return profileImageData; }
            set
            {
                profileImageData = value;
                NotifyOfPropertyChange(() => ProfileImageData);
            }
        }
        private byte[] profileImageBannerImageData;
        [JsonIgnore]
        public byte[] ProfileBannerImageData
        {
            get { return profileImageBannerImageData; }
            set
            {
                profileImageBannerImageData = value;
                NotifyOfPropertyChange(() => ProfileBannerImageData);
            }
        }
        #endregion
    }
}