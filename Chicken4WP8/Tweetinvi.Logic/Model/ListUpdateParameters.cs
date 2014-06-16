using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Logic.Model
{
    public class ListUpdateParameters : IListUpdateParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PrivacyMode PrivacyMode { get; set; }
    }
}