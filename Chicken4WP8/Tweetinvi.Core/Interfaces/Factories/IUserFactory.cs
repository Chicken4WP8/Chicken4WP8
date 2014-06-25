using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IUserFactoryAsync
    {
        Task<ILoggedUser> GetLoggedUserAsync();
        Task<ILoggedUser> GetLoggedUserAsync(IOAuthCredentials credentials);

        Task<IUser> GetUserFromIdAsync(long userId);
        Task<IUser> GetUserFromScreenNameAsync(string userName);

        // Generate User from Json
        //Task<IUser> GenerateUserFromJsonAsync(string jsonUser);

        // Get Multiple users
        Task<IEnumerable<IUser>> GetUsersFromIdsAsync(IEnumerable<long> userIds);
        Task<IEnumerable<IUser>> GetUsersFromNamesAsync(IEnumerable<string> userNames);

        // Generate user from DTO
        //Task<IUser> GenerateUserFromDTOAsync(IUserDTO userDTO);
        //Task<ILoggedUser> GenerateLoggedUserFromDTOAsync(IUserDTO userDTO);
        //Task<IEnumerable<IUser>> GenerateUsersFromDTOAsync(IEnumerable<IUserDTO> usersDTO);

        //// Generate userIdentifier from
        //Task<IUserIdentifier> GetUserIdentifierFromUserAsync(IUser user);
        //Task<IUserIdentifier> GenerateUserIdentifierFromIdAsync(long userId);
        //Task<IUserIdentifier> GenerateUserIdentifierFromScreenNameAsync(string userScreenName);
    }

    public interface IUserFactory : IUserFactoryAsync
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