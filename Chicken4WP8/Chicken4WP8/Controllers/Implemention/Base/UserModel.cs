using System;
using System.Windows.Media;
using Caliburn.Micro;
using CoreTweet;
using Newtonsoft.Json;

namespace Chicken4WP8.Controllers.Implemention.Base
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
            IsVerified = user.IsVerified;
            IsPrivate = user.IsProtected;
            IsTranslator = user.IsTranslator;
            if (user.ProfileImageUrl != null)
                ProfileImageUrl = user.ProfileImageUrl.AbsoluteUri.Replace("_normal", "_bigger");
        }

        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsTranslator { get; set; }
        /// <summary>
        /// default, use bigger avatar
        /// </summary>
        public string ProfileImageUrl { get; set; }
        private ImageSource profileImage;
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }
    }

    public class FriendModel : UserModel, IFriendModel
    {
        public FriendModel()
        { }

        public FriendModel(User user)
            : base(user)
        {
            Description = user.Description;
        }

        public string Description { get; set; }
    }

    public class ProfileModel : FriendModel, IProfileModel
    {
        public ProfileModel()
        { }

        public ProfileModel(User user)
            : base(user)
        {
            Location = user.Location;
            TweetsCount = user.StatusesCount;
            FollowingCount = user.FriendsCount;
            FollowersCount = user.FollowersCount;
            FavoritesCount = user.FavouritesCount;
            if (user.Url != null)
                Url = user.Url.AbsoluteUri;
            if (user.ProfileBannerUrl != null)
                UserProfileBannerImage = user.ProfileBannerUrl.AbsoluteUri;
            if (user.Entities != null)
                UserEntities = new UserEntititesModel(user.Entities);
        }

        public string UserProfileBannerImage { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public IUserEntities UserEntities { get; set; }
        public int TweetsCount { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
        public int FavoritesCount { get; set; }

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
    }
}