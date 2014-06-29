using System.Threading.Tasks;

namespace Chicken4WP8.Services.Interface
{
    public interface IProgressService
    {
        void Show();
        void Show(string text);
        void Hide();
        Task ShowAsync();
        Task ShowAsync(string text);
        Task HideAsync();
    }
}
