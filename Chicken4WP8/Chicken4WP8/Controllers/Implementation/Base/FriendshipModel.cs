using CoreTweet;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class FriendshipModel : IFriendshipModel
    {
        public FriendshipModel()
        { }

        public FriendshipModel(Friendship friendship)
        {
            if (friendship != null)
            {
                Id = friendship.Id;
                ScreenName = friendship.ScreenName;
                Connections = friendship.Connections;
            }
        }

        public long Id { get; set; }
        public string ScreenName { get; set; }
        //bool? AllReplies { get; set; }
        //bool? CanDM { get; set; }
        public string[] Connections { get; set; }
        //bool? IsBlocking { get; set; }
        //bool? IsFollowedBy { get; set; }
        //bool? IsFollowing { get; set; }
        //bool? IsMarkedSpam { get; set; }
        //bool? IsMuting { get; set; }
        //bool? IsNotificationsEnabled { get; set; }
        //bool? WantsRetweets { get; set; }
    }
}
