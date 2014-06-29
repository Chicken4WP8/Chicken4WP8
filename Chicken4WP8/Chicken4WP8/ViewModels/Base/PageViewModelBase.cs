using Caliburn.Micro;

namespace Chicken4WP8.ViewModels.Base
{
    /// <summary>
    /// for page and pivot
    /// </summary>
    public abstract class PageViewModelBase : Screen
    {
        public PageViewModelBase()
        {
        }

        public string Random { get; set; }
    }
}
