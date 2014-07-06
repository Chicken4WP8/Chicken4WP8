using System.ComponentModel;
using System.Windows.Media;

namespace Chicken4WP8.Controllers
{
    public interface IImageSource : INotifyPropertyChanged
    {
        ImageSource ImageSource { get; set; }
    }
}
