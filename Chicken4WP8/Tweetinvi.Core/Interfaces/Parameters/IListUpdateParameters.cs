using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IListUpdateParameters
    {
        string Name { get; set; }
        string Description { get; set; }
        PrivacyMode PrivacyMode { get; set; }
    }
}