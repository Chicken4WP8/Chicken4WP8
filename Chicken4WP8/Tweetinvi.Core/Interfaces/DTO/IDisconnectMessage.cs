namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IDisconnectMessage
    {
        int Code { get; set; }
        string StreamName { get; set; }
        string Reason { get; set; }
    }
}