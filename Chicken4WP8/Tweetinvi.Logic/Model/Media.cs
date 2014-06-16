using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.Model
{
    public class Media : IMedia
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}