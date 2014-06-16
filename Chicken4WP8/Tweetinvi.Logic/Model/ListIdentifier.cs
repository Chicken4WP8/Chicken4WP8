using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Logic.Model
{
    public class ListIdentifierFactory : IListIdentifierFactory
    {
        private readonly IFactory<IListIdentifier> _listIdentifierUnityFactory;
        private readonly IUserQueryValidator _userQueryValidator;

        public ListIdentifierFactory(
            IFactory<IListIdentifier> listIdentifierUnityFactory,
            IUserQueryValidator userQueryValidator)
        {
            _listIdentifierUnityFactory = listIdentifierUnityFactory;
            _userQueryValidator = userQueryValidator;
        }

        public IListIdentifier Create(ITweetListDTO tweetListDTO)
        {
            if (tweetListDTO == null)
            {
                return null;
            }

            if (tweetListDTO.Id != TweetinviConfig.DEFAULT_ID)
            {
                return Create(tweetListDTO.Id);
            }

            if (!String.IsNullOrEmpty(tweetListDTO.Slug) && _userQueryValidator.CanUserBeIdentified(tweetListDTO.Creator))
            {
                if (_userQueryValidator.IsUserIdValid(tweetListDTO.Creator.Id))
                {
                    return Create(tweetListDTO.Slug, tweetListDTO.Creator.Id);
                }

                return Create(tweetListDTO.Slug, tweetListDTO.Creator.ScreenName);
            }

            return null;
        }

        public IListIdentifier Create(long listId)
        {
            var listIdentifier = _listIdentifierUnityFactory.Create();
            listIdentifier.ListId = listId;
            return listIdentifier;
        }

        public IListIdentifier Create(string slug, IUserIdentifier userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            if (userDTO.Id != TweetinviConfig.DEFAULT_ID)
            {
                return Create(slug, userDTO.Id);
            }

            if (!String.IsNullOrEmpty(userDTO.ScreenName))
            {
                return Create(slug, userDTO.ScreenName);
            }

            return null;
        }

        public IListIdentifier Create(string slug, long ownerId)
        {
            var listIdentifier = _listIdentifierUnityFactory.Create();
            listIdentifier.Slug = slug;
            listIdentifier.OwnerId = ownerId;
            return listIdentifier;
        }

        public IListIdentifier Create(string slug, string ownerScreenName)
        {
            var listIdentifier = _listIdentifierUnityFactory.Create();
            listIdentifier.Slug = slug;
            listIdentifier.OwnerScreenName = ownerScreenName;
            return listIdentifier;
        }
    }

    public class ListIdentifier : IListIdentifier
    {
        public ListIdentifier()
        {
            ListId = TweetinviConfig.DEFAULT_ID;
            OwnerId = TweetinviConfig.DEFAULT_ID;
        }

        public long ListId { get; set; }
        public string Slug { get; set; }
        public long OwnerId { get; set; }
        public string OwnerScreenName { get; set; }
    }
}