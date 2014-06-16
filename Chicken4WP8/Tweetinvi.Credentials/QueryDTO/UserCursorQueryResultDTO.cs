using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Credentials.QueryDTO
{
    public class UserCursorQueryResultDTO : BaseCursorQueryDTO, IUserCursorQueryResultDTO
    {
        public IUserDTO[] Users { get; set; }

        public override int GetNumberOfObjectRetrieved()
        {
            return Users.Length;
        }
    }
}