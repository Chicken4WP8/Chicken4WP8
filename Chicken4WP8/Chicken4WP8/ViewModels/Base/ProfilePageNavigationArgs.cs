using Chicken4WP8.Controllers;

namespace Chicken4WP8.ViewModels.Base
{
    public class ProfilePageNavigationArgs
    {
        public ProfilePageNavigationArgs()
        { }

        public IUserModel User { get; set; }
        public IUserMentionEntity Mention { get; set; }
    }
}
