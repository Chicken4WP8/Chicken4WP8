namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IDisconnectMessageDTO
    {
        int Code { get; set; }
        string StreamName { get; set; }
        string Reason { get; set; }
    }
}