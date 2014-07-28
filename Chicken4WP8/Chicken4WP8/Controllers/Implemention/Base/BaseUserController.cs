using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
        public BaseUserController()
        { }

        public async Task<byte[]> DownloadProfileImageAsync(string url)
        {
            return await base.DownloadImage(url);
        }

        public async Task SetProfileImageAsync(IUserModel user, byte[] data)
        {
            await Task.Factory.StartNew(() => base.SetImageFromBytes(user, data));
        }
    }
}
