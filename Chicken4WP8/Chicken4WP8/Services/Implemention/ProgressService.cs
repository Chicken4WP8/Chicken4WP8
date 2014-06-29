using System.Threading.Tasks;
using System.Windows;
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
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    progressIndicator.Text = text;
                    progressIndicator.IsIndeterminate = true;
                    progressIndicator.IsVisible = true;
                });
        }

        public void Hide()
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    progressIndicator.IsIndeterminate = false;
                    progressIndicator.IsVisible = false;
                });
        }

        #endregion

        #region async
        public async Task ShowAsync()
        {
            await Task.Factory.StartNew(Show);
        }

        public async Task ShowAsync(string text)
        {
            await Task.Factory.StartNew(() => Show(text));
        }

        public async Task HideAsync()
        {
            await Task.Factory.StartNew(Hide);
        }
        #endregion
    }
}
