using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers.Interface;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.ViewModels.Base;
using Chicken4WP8.ViewModels.Home;
using Chicken4WP8.Views.Status;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Status
{
    public class NewStatusPageViewModel : PageViewModelBase
    {
        #region properties
        private PhoneTextBox textbox;

        private string title;
        public string Title
        {
            get
            { return title; }
            set
            {
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        private readonly WaitCursor waitCursorService;

        public TextViewModel TextViewModel { get; set; }
        public EmotionViewModel EmotionViewModel { get; set; }
        public IStatusController StatusController { get; set; }
        public INavigationService NavigationService { get; set; }
        public ILanguageHelper LanguageHelper { get; set; }
        #endregion

        public NewStatusPageViewModel()
        {
            waitCursorService = WaitCursorService.WaitCursor;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var control = view as NewStatusPageView;
            textbox = control.Text;
            textbox.GotFocus += Textbox_GotFocus;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            EmotionViewModel.AddEmotionHandler = this.AddEmotion;

            Items.Add(TextViewModel);
            Items.Add(EmotionViewModel);

            ActivateItem(TextViewModel);
        }

        public async void AppBar_Send()
        {
            if (string.IsNullOrEmpty(Text))
                return;
            var options = Const.GetDictionary();
            options.Add(Const.STATUS, Text);
            waitCursorService.IsVisible = true;
            waitCursorService.Text = LanguageHelper["WaitCursor_SendNewTweet"];
            await StatusController.UpdateAsync(options);
            waitCursorService.IsVisible = false;
            NavigationService.UriFor<HomePageViewModel>().Navigate();
        }

        private void Textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            ActivateItem(TextViewModel);
        }

        private void AddEmotion(string emotion)
        {
            if (string.IsNullOrEmpty(this.textbox.Text))
            {
                this.textbox.Text = emotion;
                this.textbox.SelectionStart = emotion.Length;
                return;
            }
            if (this.textbox.Text.Length + emotion.Length > this.textbox.MaxLength)
                return;
            int start = this.textbox.SelectionStart;
            this.textbox.Text = this.textbox.Text.Insert(start, emotion);
            this.textbox.SelectionStart = start + emotion.Length;
        }
    }
}
