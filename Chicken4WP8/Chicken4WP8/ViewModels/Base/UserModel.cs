using System;
using System.Windows.Media;
using Caliburn.Micro;
using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8.ViewModels.Base
{
    public class UserModel : PropertyChangedBase
    {
        #region private
        //private static ImageSource defaultImage = new BitmapImage
        private IUser user;
        #endregion

        public UserModel(IUser user)
        {
            this.user = user;
        }

        public IUser User
        {
            get { return user; }
        }

        public long Id
        {
            get { return user.Id; }
        }

        public string Name
        {
            get { return user.Name; }
        }

        public string ScreenName
        {
            get { return user.ScreenName; }
        }

        public string Description
        {
            get { return user.Description; }
        }

        public DateTime CreatedAt
        {
            get { return user.CreatedAt; }
        }

        public string Location
        {
            get { return user.Location; }
        }

        public bool IsFollowing
        {
            get { return user.Following; }
        }

        public bool IsVerified
        {
            get { return user.Verified; }
        }

        public bool IsPrivate
        {
            get { return user.Protected; }
        }

        public bool IsTranslator
        {
            get { return user.IsTranslator; }
        }

        public string ProfileImageUrl
        {
            get { return user.ProfileImageUrl; }
        }

        public string ProfileImageUrlHttps
        {
            get { return user.ProfileImageUrlHttps; }
        }

        private ImageSource profileImage;
        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                NotifyOfPropertyChange(() => ProfileImage);
            }
        }
    }
}