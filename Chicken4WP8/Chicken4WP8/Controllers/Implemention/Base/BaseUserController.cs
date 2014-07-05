using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class BaseUserController : BaseControllerBase, IUserController
    {
        public async Task SetProfileImageStreamAsync(IUserModel user)
        {
            //get from database

            //then from web
            var data = await base.DownloadImage(user.ProfileImageUrl);
            base.SetImageFromBytes(user, data);
        }
    }
}
