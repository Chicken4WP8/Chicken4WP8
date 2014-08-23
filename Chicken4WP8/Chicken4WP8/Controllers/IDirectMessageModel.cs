using System;

namespace Chicken4WP8.Controllers
{
    public interface IDirectMessageModel
    {
        DateTime CreatedAt { get; set; }
        IEntities Entities { get; set; }
        long Id { get; set; }
        IUserModel User { get; set; }
        string Text { get; set; }
    }
}
