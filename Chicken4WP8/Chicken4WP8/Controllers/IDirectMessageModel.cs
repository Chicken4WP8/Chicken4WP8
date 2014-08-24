using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chicken4WP8.Controllers
{
    public interface IDirectMessageModel : INotifyPropertyChanged
    {
        DateTime CreatedAt { get; set; }
        IEntities Entities { get; set; }
        long Id { get; set; }
        IUserModel User { get; set; }
        string Text { get; set; }
        bool IsSentByMe { get; set; }
        #region for template
        bool IncludeMedia { get; }
        List<IEntity> ParsedEntities { get; } 
        #endregion
    }
}
