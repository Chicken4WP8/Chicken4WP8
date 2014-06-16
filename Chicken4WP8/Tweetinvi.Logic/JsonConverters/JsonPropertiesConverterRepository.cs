using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.TwitterEntities;

namespace Tweetinvi.Logic.JsonConverters
{
    /// <summary>
    /// Repository of multiple converters required to generate DTO classes.
    /// </summary>
    public class JsonPropertiesConverterRepository
    {
        public static JsonConverter[] Converters { get; private set; }

        static JsonPropertiesConverterRepository()
        {
            Initialize();
        }

        private static void Initialize()
        {
            var converters = new List<JsonConverter>
            {
                new JsonInterfaceToObjectConverter<ITweetDTO, TweetDTO>(),
                new JsonInterfaceToObjectConverter<ITweetListDTO, TweetListDTO>(),
                new JsonInterfaceToObjectConverter<IOEmbedTweetDTO, OEmbedTweetDTO>(),
                new JsonInterfaceToObjectConverter<IUserDTO, UserDTO>(),
                new JsonInterfaceToObjectConverter<IMessageDTO, MessageDTO>(),

                new JsonInterfaceToObjectConverter<IRelationshipDTO, RelationshipDTO>(),
                new JsonInterfaceToObjectConverter<IRelationshipStateDTO, RelationshipStateDTO>(),

                new JsonInterfaceToObjectConverter<IAccountSettingsDTO, AccountSettingsDTO>(),
                new JsonInterfaceToObjectConverter<ILocation, Location>(),
                new JsonInterfaceToObjectConverter<IPlace, Place>(),
                
                new JsonInterfaceToObjectConverter<IUrlEntity, UrlEntity>(),
                new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>(),
                new JsonInterfaceToObjectConverter<IMediaEntity, MediaEntity>(),
                new JsonInterfaceToObjectConverter<IMediaEntitySize, MediaEntitySize>(),
                new JsonInterfaceToObjectConverter<IUserMentionEntity, UserMentionEntity>(),
                new JsonInterfaceToObjectConverter<IDescriptionEntity, DescriptionEntity>(),
                new JsonInterfaceToObjectConverter<IWebsiteEntity, WebsiteEntity>(),

                new JsonInterfaceToObjectConverter<IUserEntities, UserEntities>(),
                new JsonInterfaceToObjectConverter<ITweetEntities, TweetEntities>(),

                new JsonInterfaceToObjectConverter<IRelationship, Relationship>(),
                new JsonInterfaceToObjectConverter<IRelationshipState, RelationshipState>(),
                
                new JsonInterfaceToObjectConverter<IPlaceTrends, PlaceTrends>(),
                new JsonInterfaceToObjectConverter<ITrend, Trend>(),
                new JsonInterfaceToObjectConverter<IWoeIdLocation, WoeIdLocation>(),
                
                new JsonInterfaceToObjectConverter<ITokenRateLimit, TokenRateLimit>(),
                new JsonInterfaceToObjectConverter<ITokenRateLimits, TokenRateLimits>(),
                new JsonInterfaceToObjectConverter<ISavedSearchDTO, SavedSearchDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterExceptionInfo, TwitterExceptionInfo>(),

                // JsonCoordinatesConverter is used only for Properties (with an s) and not Property
                // because Twitter does not provide the coordinates the same way if it is a list or
                // if it is a single argument.
                new JsonCoordinatesConverter(),
            };

            Converters = converters.ToArray();
        }
    }
}