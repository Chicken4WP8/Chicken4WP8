using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IMessageController
    {
        IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = 40);
        IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = 40);

        // Publish Message
        IMessage PublishMessage(IMessage message);
        IMessage PublishMessage(IMessageDTO messageDTO);
        IMessage PublishMessage(string text, IUser targetUser);
        IMessage PublishMessage(string text, IUserIdentifier targetUserDTO);
        IMessage PublishMessage(string text, long targetUserId);
        IMessage PublishMessage(string text, string targetUserScreenName);

        // Destroy Message
        bool DestroyMessage(IMessage message);
        bool DestroyMessage(IMessageDTO messageDTO);
        bool DestroyMessage(long messageId);
    }
}