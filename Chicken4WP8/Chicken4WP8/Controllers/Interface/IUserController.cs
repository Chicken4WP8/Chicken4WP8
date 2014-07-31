using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IUserController
    {
        Task SetProfileImageAsync(IUserModel user);
        Task SetProfileBannerImageAsync(IUserModel user);
    }
}
