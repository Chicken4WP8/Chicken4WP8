using Caliburn.Micro;

namespace Chicken4WP8.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        public ViewModelBase()
        {
        }


        public string Random { get; set; }
    }
}
