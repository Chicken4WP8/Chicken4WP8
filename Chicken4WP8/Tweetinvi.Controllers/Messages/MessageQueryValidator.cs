using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryValidator
    {
        bool IsMessageTextValid(string message);
        bool IsMessageIdValid(long messageId);

        bool CanMessageDTOBePublished(IMessageDTO messageDTO);
        bool CanMessageDTOBeDestroyed(IMessageDTO messageDTO);
    }

    public class MessageQueryValidator : IMessageQueryValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public MessageQueryValidator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public bool IsMessageTextValid(string message)
        {
            return !String.IsNullOrEmpty(message);
        }

        public bool IsMessageIdValid(long messageId)
        {
            return messageId != TweetinviConfig.DEFAULT_ID;
        }

        public bool CanMessageDTOBePublished(IMessageDTO messageDTO)
        {
            bool isMessageInValidState = messageDTO != null &&
                                         !messageDTO.IsMessagePublished &&
                                         !messageDTO.IsMessageDestroyed;

            if (!isMessageInValidState)
            {
                return false;
            }

            bool isRecipientValid = _userQueryValidator.CanUserBeIdentified(messageDTO.Recipient) ||
                                    _userQueryValidator.IsUserIdValid(messageDTO.RecipientId) ||
                                    _userQueryValidator.IsScreenNameValid(messageDTO.RecipientScreenName);

            bool messageTextIsValid = IsMessageTextValid(messageDTO.Text);

            return isRecipientValid && messageTextIsValid;
        }

        public bool CanMessageDTOBeDestroyed(IMessageDTO messageDTO)
        {
            bool isMessageInValidStateToBeDestroyed = messageDTO != null &&
                                                      _userQueryValidator.IsUserIdValid(messageDTO.Id) &&
                                                      messageDTO.IsMessagePublished &&
                                                      !messageDTO.IsMessageDestroyed;

            return isMessageInValidStateToBeDestroyed;
        }
    }
}