using System.Threading.Tasks;
using System.Windows.Navigation;
using Chicken4WP8.Services.Interface;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken4WP8.Services.Implemention
{
    public class ProgressService : IProgressService
    {
        private readonly ProgressIndicator progressIndicator;
        public ILanguageHelper LanguageHelper { get; set; }

        public ProgressService(PhoneApplicationFrame rootFrame)
        {
            progressIndicator = new ProgressIndicator();
            rootFrame.Navigated += RootFrameOnNavigated;
        }

        private void RootFrameOnNavigated(object sender, NavigationEventArgs args)
        {
            var content = args.Content;
            var page = content as PhoneApplicationPage;
            if (page == null)
                return;

            page.SetValue(SystemTray.ProgressIndicatorProperty, progressIndicator);
        }

        #region sync
        public void Show()
        {
            Show(LanguageHelper.GetString("ProgressBar_Loading"));
        }

        public void Show(string text)
        {
            progressIndicator.Text = text;
            progressIndicator.IsIndeterminate = true;
            progressIndicator.IsVisible = true;
        }

        public void Hide()
        {
            progressIndicator.IsIndeterminate = false;
            progressIndicator.IsVisible = false;
        }

        #endregion

        #region async
        public async void ShowAsync()
        {
            await Task.Factory.StartNew(Show);
        }

        public async void ShowAsync(string text)
        {
            await Task.Factory.StartNew(() => Show(text));
        }

        public async void HideAsync()
        {
            await Task.Factory.StartNew(Hide);
        }
        #endregion
    }
}
