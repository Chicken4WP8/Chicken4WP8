using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IWarningMessageTooManyFollowers : IWarningMessage
    {
        IEnumerable<long> UserIds { get; }
    }
}