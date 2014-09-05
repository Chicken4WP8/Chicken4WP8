
namespace Chicken4WP8.Controllers
{
    public class NewStatusModel
    {
        NewStatusType Type { get; set; }
        long? InReplyToStatusId { get; set; }
        string InReplyToUserName { get; set; }
        string Text { get; set; }
    }

    public enum NewStatusType
    {
        None=0,
        New =1,
        Reply=2,
        Quote =3,
    }
}
