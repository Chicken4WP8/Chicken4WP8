
namespace Chicken4WP8.Services.Interface
{
    public interface IProgressService
    {
        void Show();
        void ShowAsync();
        void Show(string text);
        void ShowAsync(string text);
        void Hide();
        void HideAsync();
    }
}
