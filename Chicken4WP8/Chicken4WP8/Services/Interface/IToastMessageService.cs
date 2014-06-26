using System;
using System.Threading.Tasks;

namespace Chicken4WP8.Services.Interface
{
    public interface IToastMessageService
    {
        void HandleMessage(string message, Action complete = null);
    }
}
