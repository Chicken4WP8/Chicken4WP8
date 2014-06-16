using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;

namespace Tweetinvi.Credentials.QueryDTO
{
    public class IdsCursorQueryResultDTO : BaseCursorQueryDTO, IIdsCursorQueryResultDTO
    {
        [JsonProperty("ids")]
        public long[] Ids { get; set; }

        public override int GetNumberOfObjectRetrieved()
        {
            return Ids.Length;
        }
    }
}