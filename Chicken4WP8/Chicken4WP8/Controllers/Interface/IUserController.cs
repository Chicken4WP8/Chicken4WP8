using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IUserController
    {
        Task SetProfileImageStreamAsync(IUserModel user);
    }
}
