using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryGenerator
    {
        // Get messages
        string GetLatestMessagesReceivedQuery(int maximumMessages);
        string GetLatestMessagesSentQuery(int maximumMessages);

        // Publish Message
        string GetPublishMessageQuery(IMessageDTO messageDTO);
        string GetPublishMessageQuery(string messageText, IUserIdentifier targetUserDTO);
        string GetPublishMessageQuery(string messageText, string targetUserScreenName);
        string GetPublishMessageQuery(string messageText, long targetUserId);

        // Detroy Message
        string GetDestroyMessageQuery(IMessageDTO messageDTO);
        string GetDestroyMessageQuery(long messageId);
    }

    public class MessageQueryGenerator : IMessageQueryGenerator
    {
        private readonly IMessageQueryValidator _messageQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public MessageQueryGenerator(
            IMessageQueryValidator messageQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _messageQueryValidator = messageQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        // Get collection of messages
        public string GetLatestMessagesReceivedQuery(int maximumMessages)
        {
            return String.Format(Resources.Message_GetMessagesReceived, maximumMessages);
        }

        public string GetLatestMessagesSentQuery(int maximumMessages)
        {
            return String.Format(Resources.Messages_GetMessagesSent, maximumMessages);
        }

        // Publish Message
        public string GetPublishMessageQuery(IMessageDTO messageDTO)
        {
            if (!_messageQueryValidator.CanMessageDTOBePublished(messageDTO))
            {
                return null;
            }

            if (_userQueryValidator.CanUserBeIdentified(messageDTO.Recipient))
            {
                return GetPublishMessageQuery(messageDTO.Text, messageDTO.Recipient);
            }

            if (_userQueryValidator.IsUserIdValid(messageDTO.RecipientId))
            {
                return GetPublishMessageQuery(messageDTO.Text, messageDTO.RecipientId);
            }

            return GetPublishMessageQuery(messageDTO.Text, messageDTO.RecipientScreenName);
        }

        public string GetPublishMessageQuery(string messageText, IUserIdentifier targetUserDTO)
        {
            if (!_messageQueryValidator.IsMessageTextValid(messageText) || !_userQueryValidator.CanUserBeIdentified(targetUserDTO))
            {
                return null;
            }

            string identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(targetUserDTO);
            return GetPublishMessageFormattedQuery(messageText, identifierParameter);
        }

        public string GetPublishMessageQuery(string messageText, string targetUserScreenName)
        {
            if (!_messageQueryValidator.IsMessageTextValid(messageText) || !_userQueryValidator.IsScreenNameValid(targetUserScreenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(targetUserScreenName);
            return GetPublishMessageFormattedQuery(messageText, userScreenNameParameter);
        }

        public string GetPublishMessageQuery(string messageText, long targetUserId)
        {
            if (!_messageQueryValidator.IsMessageTextValid(messageText) || !_userQueryValidator.IsUserIdValid(targetUserId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(targetUserId);
            return GetPublishMessageFormattedQuery(messageText, userIdParameter);
        }

        private string GetPublishMessageFormattedQuery(string message, string userIdentifier)
        {
            return String.Format(Resources.Message_NewMessage, _twitterStringFormatter.TwitterEncode(message), userIdentifier);
        }

        public string GetDestroyMessageQuery(IMessageDTO messageDTO)
        {
            if (!_messageQueryValidator.CanMessageDTOBeDestroyed(messageDTO))
            {
                return null;
            }

            return GetDestroyMessageQuery(messageDTO.Id);
        }

        // Destroy Message
        public string GetDestroyMessageQuery(long messageId)
        {
            if (!_messageQueryValidator.IsMessageIdValid(messageId))
            {
                return null;
            }

            return String.Format(Resources.Message_DestroyMessage, messageId);
        }
    }
}