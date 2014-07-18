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
            Description = user.Description;
            CreatedAt = user.CreatedAt.LocalDateTime;
            Location = user.Location;
            IsVerified = user.IsVerified;
            IsPrivate = user.IsProtected;
            IsTranslator = user.IsTranslator;
            if (user.ProfileImageUrl != null)
                ProfileImageUrl = user.ProfileImageUrl.AbsoluteUri.Replace("_normal", "_bigger");
        }

        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
        public bool IsFollowing { get; set; }
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
}