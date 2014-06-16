namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IWarningMessageFallingBehind : IWarningMessage
    {
        int PercentFull { get; }
    }
}