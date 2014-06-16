using System.Globalization;

namespace Chicken4WP8.Services.Interface
{
    public interface ILanguageService
    {
        void SetLanguage(CultureInfo cultureInfo);
        string this[string key] { get; }
        string GetString(string key, params string[] parameters);
    }
}
