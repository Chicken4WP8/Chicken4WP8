using System.Threading.Tasks;

namespace Chicken4WP8.Controllers.Interface
{
    public interface IUserController
    {
        Task<byte[]> DownloadProfileImageAsync(string url);
        Task SetProfileImageAsync(IUserModel user, byte[] data);
    }
}
