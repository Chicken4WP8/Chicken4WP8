
namespace Chicken4WP8.Controllers
{
    public class NewTweetModel
    {
        public NewTweetType Type { get; set; }
        public long? InReplyToStatusId { get; set; }
        public string InReplyToUserName { get; set; }
        public string Text { get; set; }
    }

    public enum NewTweetType
    {
        None = 0,
        New = 1,
        Reply = 2,
        Quote = 3,
    }
}
