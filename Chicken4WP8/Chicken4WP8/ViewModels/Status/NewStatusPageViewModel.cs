using System.Windows;
using System.Windows.Controls;
using Chicken4WP8.ViewModels.Base;
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

        public TextViewModel TextViewModel { get; set; }
        public EmotionViewModel EmotionViewModel { get; set; }
        #endregion

        public NewStatusPageViewModel()
        {
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var control = view as FrameworkElement;
            textbox = control.GetFirstLogicalChildByType<PhoneTextBox>(true);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            EmotionViewModel.AddEmotionHandler = this.AddEmotion;

            Items.Add(TextViewModel);
            Items.Add(EmotionViewModel);

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
