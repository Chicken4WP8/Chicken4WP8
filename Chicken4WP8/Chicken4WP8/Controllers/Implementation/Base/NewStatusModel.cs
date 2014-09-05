
namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class NewStatusModel : INewStatusModel
    {
        public NewStatusType Type { get; set; }
        public long? InReplyToStatusId { get; set; }
        public string InReplyToUserName { get; set; }
        public string Text { get; set; }
    }
}
