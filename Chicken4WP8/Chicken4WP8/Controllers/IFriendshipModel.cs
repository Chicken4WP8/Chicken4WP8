
namespace Chicken4WP8.Controllers
{
    public interface IFriendshipModel
    {
        long Id { get; set; }
        string ScreenName { get; set; }
        //bool? AllReplies { get; set; }
        //bool? CanDM { get; set; }
        string[] Connections { get; set; }
        //bool? IsBlocking { get; set; }
        //bool? IsFollowedBy { get; set; }
        //bool? IsFollowing { get; set; }
        //bool? IsMarkedSpam { get; set; }
        //bool? IsMuting { get; set; }
        //bool? IsNotificationsEnabled { get; set; }
        //bool? WantsRetweets { get; set; }
    }
}
