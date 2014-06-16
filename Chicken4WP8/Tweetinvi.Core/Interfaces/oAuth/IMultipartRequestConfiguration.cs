using System.Text;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    public interface IMultipartRequestConfiguration
    {
        string Boundary { get; set; }
        string StartBoundary { get; set; }
        string EndBoundary { get; set; }
        Encoding EncodingAlgorithm { get; set; }
    }
}