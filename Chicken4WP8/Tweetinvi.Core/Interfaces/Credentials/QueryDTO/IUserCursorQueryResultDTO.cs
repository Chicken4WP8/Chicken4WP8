using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Credentials.QueryDTO
{
    public interface IUserCursorQueryResultDTO : IBaseCursorQueryDTO
    {
        IUserDTO[] Users { get; set; }
    }
}