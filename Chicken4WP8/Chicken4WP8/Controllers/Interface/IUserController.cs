using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IUserController
    {
        Task<byte[]> DownloadProfileImageAsync(IUserModel user);
        Task SetProfileImageAsync(IUserModel user, byte[] data);
    }
}
