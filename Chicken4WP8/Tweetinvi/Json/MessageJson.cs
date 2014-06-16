using System;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Json
{
    public static class MessageJson
    {
        [ThreadStatic]
        private static IMessageJsonController _messageJsonController;
        public static IMessageJsonController MessageJsonController
        {
            get
            {
                if (_messageJsonController == null)
                {
                    Initialize();
                }
                
                return _messageJsonController;
            }
        }

        static MessageJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _messageJsonController = TweetinviContainer.Resolve<IMessageJsonController>();
        }

        // Get Messages
        public static string GetLatestMessagesReceived(int maximumMessages = 40)
        {
            return MessageJsonController.GetLatestMessagesReceived(maximumMessages);
        }

        public static string GetLatestMessagesSent(int maximumMessages = 40)
        {
            return MessageJsonController.GetLatestMessagesSent(maximumMessages);
        }

        // Publish Message
        public static string PublishMessage(IMessage message)
        {
            return MessageJsonController.PublishMessage(message);
        }

        public static string PublishMessage(IMessageDTO messageDTO)
        {
            return MessageJsonController.PublishMessage(messageDTO);
        }

        public static string PublishMessage(string text, IUserIdentifier targetUserDTO)
        {
            return MessageJsonController.PublishMessage(text, targetUserDTO);
        }

        public static string PublishMessage(string text, long targetUserId)
        {
            return MessageJsonController.PublishMessage(text, targetUserId);
        }

        public static string PublishMessage(string text, string targetUserScreenName)
        {
            return MessageJsonController.PublishMessage(text, targetUserScreenName);
        }

        // Destroy Message
        public static string DestroyMessage(IMessage message)
        {
            return MessageJsonController.DestroyMessage(message);
        }

        public static string DestroyMessage(IMessageDTO messageDTO)
        {
            return MessageJsonController.DestroyMessage(messageDTO);
        }

        public static string DestroyMessage(long messageId)
        {
            return MessageJsonController.DestroyMessage(messageId);
        }
    }
}