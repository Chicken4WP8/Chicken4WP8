using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public class MessageAsync
    {
        // Factory
        public static async Task<IMessage> GetExistingMessage(long messageId)
        {
            return await Sync.ExecuteTaskAsync(() => Message.GetExistingMessage(messageId));
        }

        // Controller
        public static async Task<IEnumerable<IMessage>> GetLatestMessagesReceived(int maximumMessages = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Message.GetLatestMessagesReceived(maximumMessages));
        }

        public static async Task<IEnumerable<IMessage>> GetLatestMessagesSent(int maximumMessages = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Message.GetLatestMessagesSent(maximumMessages));
        }

        // Publish Message
        public static async Task<IMessage> PublishMessage(IMessage message)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(message));
        }

        public static async Task<IMessage> PublishMessage(IMessageDTO messageDTO)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(messageDTO));
        }

        public static async Task<IMessage> PublishMessage(string text, IUser targetUser)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUser));
        }

        public static async Task<IMessage> PublishMessage(string text, IUserIdentifier targetUserDTO)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUserDTO));
        }

        public static async Task<IMessage> PublishMessage(string text, long targetUserId)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUserId));
        }

        public static async Task<IMessage> PublishMessage(string text, string targetUserScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUserScreenName));
        }

        // Destroy Message
        public static async Task<bool> DestroyMessage(IMessage message)
        {
            return await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(message));
        }

        public static async Task<bool> DestroyMessage(IMessageDTO messageDTO)
        {
            return  await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(messageDTO));
        }

        public static async Task<bool> DestroyMessage(long messageId)
        {
            return  await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(messageId));
        }
    }
}
