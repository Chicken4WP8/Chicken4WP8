using System;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Chicken4WP8.Services.Interface;
using Chicken4WP8.Views.Status;
using Microsoft.Phone.Controls;

namespace Chicken4WP8.ViewModels.Status
{
    public class EmotionViewModel : Screen
    {
        private WrapPanel panel;
        public Action<string> AddEmotionHandler;
        public IStorageService StorageService { get; set; }

        public EmotionViewModel()
        { 
            DisplayName = "^_^";
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var control = view as EmotionView;
            panel = control.WrapPanel;

            var texts = StorageService.GetEmotions();
            foreach (var text in texts)
            {
                var button = new Button { Content = text };
                button.Click += this.EmotionButton_Click;
                panel.Children.Add(button);
            }
        }

        private void EmotionButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddEmotionHandler != null)
            {
                string result = (sender as Button).Content.ToString();
                AddEmotionHandler(result);
            }
        }
    }
}
