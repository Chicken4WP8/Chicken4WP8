using System;
using Caliburn.Micro;
using Tweetinvi.Core.Interfaces;

namespace Chicken4WP8.ViewModels.Base
{
    public class UserModel : PropertyChangedBase
    {
        #region private
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

        public DateTime CreatedAt
        {
            get { return user.CreatedAt; }
        }

        public string Name
        {
            get { return user.Name; }
        }

        public string ScreenName
        {
            get { return user.ScreenName; }
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
    }
}