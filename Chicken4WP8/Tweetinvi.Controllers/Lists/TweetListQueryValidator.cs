using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Lists
{
    public class TweetListQueryValidator : ITweetListQueryValidator
    {
        public bool IsListUpdateParametersValid(IListUpdateParameters parameters)
        {
            return parameters != null;
        }

        public bool IsDescriptionParameterValid(string description)
        {
            return !String.IsNullOrEmpty(description);
        }

        public bool IsNameParameterValid(string name)
        {
            return !String.IsNullOrEmpty(name);
        }

        public bool IsListIdentifierValid(IListIdentifier listIdentifier)
        {
            if (listIdentifier == null)
            {
                return false;
            }

            if (listIdentifier.ListId != TweetinviConfig.DEFAULT_ID)
            {
                return true;
            }

            bool isOwnerIdentifierValid = IsOwnerIdValid(listIdentifier.OwnerId) || IsOwnerScreenNameValid(listIdentifier.OwnerScreenName);
            return IsSlugValid(listIdentifier.Slug) && isOwnerIdentifierValid;
        }

        public bool IsOwnerScreenNameValid(string ownerScreenName)
        {
            return !String.IsNullOrEmpty(ownerScreenName);
        }

        public bool IsOwnerIdValid(long ownderId)
        {
            return ownderId != 0;
        }

        public bool IsSlugValid(string slug)
        {
            return !String.IsNullOrEmpty(slug);
        }
    }
}