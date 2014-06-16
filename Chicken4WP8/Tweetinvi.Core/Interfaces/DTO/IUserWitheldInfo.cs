using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IUserWitheldInfo
    {
        long Id { get; }
        IEnumerable<string> WitheldInCountries { get; }
    }
}