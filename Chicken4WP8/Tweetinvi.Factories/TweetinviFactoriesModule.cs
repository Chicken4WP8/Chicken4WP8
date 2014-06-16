using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Factories.Credentials;
using Tweetinvi.Factories.Friendship;
using Tweetinvi.Factories.Geo;
using Tweetinvi.Factories.Lists;
using Tweetinvi.Factories.SavedSearch;
using Tweetinvi.Factories.Tweet;
using Tweetinvi.Factories.User;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.TwitterEntities;

namespace Tweetinvi.Factories
{
    public class TweetinviFactoriesModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviFactoriesModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            RegisterDTO();
            RegisterFactories();
        }

        private void RegisterDTO()
        {
            _container.RegisterType<ITweetDTO, TweetDTO>();
            _container.RegisterType<ITweetListDTO, TweetListDTO>();
            _container.RegisterType<IUserDTO, UserDTO>();
            _container.RegisterType<IUserIdentifier, UserIdentifier>();
            _container.RegisterType<IMessageDTO, MessageDTO>();
            _container.RegisterType<IRelationshipDTO, RelationshipDTO>();

            _container.RegisterType<ITweetEntities, TweetEntities>();
            _container.RegisterType<IUserEntities, UserEntities>();

            _container.RegisterType<IUrlEntity, UrlEntity>();
            _container.RegisterType<IHashtagEntity, HashtagEntity>();
            _container.RegisterType<IDescriptionEntity, DescriptionEntity>();
        }

        private void RegisterFactories()
        {
            _container.RegisterType<ICredentialsFactory, CredentialsFactory>(RegistrationLifetime.InstancePerApplication);

            _container.RegisterType<ITweetFactory, TweetFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetFactoryQueryExecutor, TweetFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IUserFactory, UserFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IUserFactoryQueryExecutor, UserFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IFriendshipFactory, FriendshipFactory>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IMessageFactory, MessageFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IMessageFactoryQueryExecutor, MessageFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ITweetListFactory, TweetListFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetListFactoryQueryExecutor, TweetListFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetListFactoryQueryGenerator, TweetListFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IGeoFactory, GeoFactory>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ISavedSearchFactory, SavedSearchFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchJsonFactory, SavedSearchJsonFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchQueryExecutor, SavedSearchFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchQueryGenerator, SavedSearchFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);
        }
    }
}