using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class MediaDTO : IMedia
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}