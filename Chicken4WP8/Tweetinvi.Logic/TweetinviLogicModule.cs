using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Logic.Helpers;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.Model.Parameters;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Logic.Wrapper;

namespace Tweetinvi.Logic
{
    public class TweetinviLogicModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviLogicModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ITweet, Tweet>();
            _container.RegisterType<IOEmbedTweet, OEmbedTweet>();
            _container.RegisterType<ITweetList, TweetList>();
            _container.RegisterType<IUser, User>();
            _container.RegisterType<ILoggedUser, LoggedUser>();
            _container.RegisterType<IAccountSettings, AccountSettings>();
            _container.RegisterType<IMessage, Message>();
            _container.RegisterType<IMention, Mention>();
            _container.RegisterType<IRelationship, Relationship>();
            _container.RegisterType<IRelationshipState, RelationshipState>();

            _container.RegisterType<ICoordinates, Coordinates>();
            _container.RegisterType<ILocation, Location>();

            _container.RegisterType<IJsonPropertyConverterRepository, JsonPropertyConverterRepository>();
            _container.RegisterType<IJsonObjectConverter, JsonObjectConverter>(RegistrationLifetime.InstancePerApplication);

            _container.RegisterType<IMedia, Media>();

            RegisterHelpers();
            InitializeWrappers();
            InitializeParameters();
            InitializeExceptionHandler();
        }

        private void RegisterHelpers()
        {
            _container.RegisterType<ITwitterStringFormatter, TwitterStringFormatter>();
        }

        private void InitializeParameters()
        {
            _container.RegisterType<IGeoCode, GeoCode>();
            _container.RegisterType<IListIdentifier, ListIdentifier>();
            _container.RegisterType<IListIdentifierFactory, ListIdentifierFactory>();
            _container.RegisterType<IListUpdateParameters, ListUpdateParameters>();
            _container.RegisterType<ITweetSearchParameters, TweetSearchParameters>();

            // Timeline
            _container.RegisterType<IHomeTimelineRequestParameters, HomeTimelineRequestParameters>();
            _container.RegisterType<IUserTimelineRequestParameters, UserTimelineRequestParameters>();
            _container.RegisterType<IMentionsTimelineRequestParameters, MentionsTimelineRequestParameters>();
        }

        private void InitializeWrappers()
        {
            _container.RegisterType<IJObjectStaticWrapper, JObjectStaticWrapper>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IJsonConvertWrapper, JsonConvertWrapper>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeExceptionHandler()
        {
            _container.RegisterType<IExceptionHandler, ExceptionHandler>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IWebExceptionInfoExtractor, WebExceptionInfoExtractor>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<ITwitterException, TwitterException>();
            _container.RegisterType<ITwitterExceptionInfo, TwitterExceptionInfo>();
        }
    }
}