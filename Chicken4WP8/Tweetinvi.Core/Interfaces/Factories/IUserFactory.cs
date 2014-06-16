using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IUserFactory
    {
        ILoggedUser GetLoggedUser();
        ILoggedUser GetLoggedUser(IOAuthCredentials credentials);

        IUser GetUserFromId(long userId);
        IUser GetUserFromScreenName(string userName);

        // Generate User from Json
        IUser GenerateUserFromJson(string jsonUser);

        // Get Multiple users
        IEnumerable<IUser> GetUsersFromIds(IEnumerable<long> userIds);
        IEnumerable<IUser> GetUsersFromNames(IEnumerable<string> userNames);

        // Generate user from DTO
        IUser GenerateUserFromDTO(IUserDTO userDTO);
        ILoggedUser GenerateLoggedUserFromDTO(IUserDTO userDTO);
        IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO);

        // Generate userIdentifier from
        IUserIdentifier GetUserIdentifierFromUser(IUser user);
        IUserIdentifier GenerateUserIdentifierFromId(long userId);
        IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName);
    }
}