using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IUserController
    {
        Task<IUserModel> ShowAsync(IDictionary<string, object> parameters);
        Task SetProfileImageAsync(IUserModel user);
        Task SetProfileImageDetailAsync(IUserModel user);
        Task SetProfileBannerImageAsync(IUserModel user);
        Task LookupFriendshipAsync(IUserModel user);
    }
}
