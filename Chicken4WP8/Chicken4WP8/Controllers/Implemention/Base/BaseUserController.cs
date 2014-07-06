using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
        public BaseUserController()
        { }

        public async Task<byte[]> DownloadProfileImageAsync(IUserModel user)
        {
            return await base.DownloadImage(user.ProfileImageUrl);
        }

        public async Task SetProfileImageAsync(IUserModel user, byte[] data)
        {
            await Task.Factory.StartNew(() => base.SetImageFromBytes(user, data));
        }
    }
}
