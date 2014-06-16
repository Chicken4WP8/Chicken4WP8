using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Factories.User
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserFactoryQueryExecutor _userFactoryQueryExecutor;
        private readonly IFactory<ILoggedUser> _loggedUserUnityFactory;
        private readonly IFactory<IUser> _userUnityFactory;
        private readonly IFactory<IUserIdentifier> _userIdentifierUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public UserFactory(
            IUserFactoryQueryExecutor userFactoryQueryExecutor,
            IFactory<ILoggedUser> loggedUserUnityFactory,
            IFactory<IUser> userUnityFactory,
            IFactory<IUserIdentifier> userIdentifierUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ICredentialsAccessor credentialsAccessor)
        {
            _userFactoryQueryExecutor = userFactoryQueryExecutor;
            _loggedUserUnityFactory = loggedUserUnityFactory;
            _userUnityFactory = userUnityFactory;
            _userIdentifierUnityFactory = userIdentifierUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _credentialsAccessor = credentialsAccessor;
        }

        // Get User
        public ILoggedUser GetLoggedUser()
        {
            var userDTO = _userFactoryQueryExecutor.GetLoggedUser();
            return GenerateLoggedUserFromDTO(userDTO);
        }

        public ILoggedUser GetLoggedUser(IOAuthCredentials credentials)
        {
            var userDTO = _credentialsAccessor.ExecuteOperationWithCredentials(credentials, () =>
            {
                return _userFactoryQueryExecutor.GetLoggedUser();
            });

            var loggedUser = GenerateLoggedUserFromDTO(userDTO);
            loggedUser.SetCredentials(credentials);

            return loggedUser;
        }

        public IUser GetUserFromId(long userId)
        {
            var userDTO = _userFactoryQueryExecutor.GetUserDTOFromId(userId);
            return GenerateUserFromDTO(userDTO);
        }

        public IUser GetUserFromScreenName(string userName)
        {
            var userDTO = _userFactoryQueryExecutor.GetUserDTOFromScreenName(userName);
            return GenerateUserFromDTO(userDTO);
        }

        // Generate User from Json
        public IUser GenerateUserFromJson(string jsonUser)
        {
            var userDTO = _jsonObjectConverter.DeserializeObject<IUserDTO>(jsonUser);
            return GenerateUserFromDTO(userDTO);
        }

        public IEnumerable<IUser> GetUsersFromIds(IEnumerable<long> userIds)
        {
            var usersDTO = _userFactoryQueryExecutor.GetUsersDTOFromIds(userIds);
            return GenerateUsersFromDTO(usersDTO);
        }

        public IEnumerable<IUser> GetUsersFromNames(IEnumerable<string> userNames)
        {
            var usersDTO = _userFactoryQueryExecutor.GetUsersDTOFromScreenNames(userNames);
            return GenerateUsersFromDTO(usersDTO);
        }

        // Generate DTO from id
        public IUserIdentifier GetUserIdentifierFromUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null");
            }

            return user.UserIdentifier;
        }

        public IUserIdentifier GenerateUserIdentifierFromId(long userId)
        {
            var userIdentifier = _userIdentifierUnityFactory.Create();
            userIdentifier.Id = userId;

            return userIdentifier;
        }

        public IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName)
        {
            var userIdentifier = _userIdentifierUnityFactory.Create();
            userIdentifier.ScreenName = userScreenName;

            return userIdentifier;
        }

        // Generate from DTO
        public ILoggedUser GenerateLoggedUserFromDTO(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            var userDTOParameterOverride = _loggedUserUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _loggedUserUnityFactory.Create(userDTOParameterOverride);

            return user;
        }

        public IUser GenerateUserFromDTO(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            var parameterOverride = _userUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _userUnityFactory.Create(parameterOverride);

            return user;
        }

        public IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            if (usersDTO == null)
            {
                return null;
            }

            return usersDTO.Select(GenerateUserFromDTO).ToList();
        }
    }
}