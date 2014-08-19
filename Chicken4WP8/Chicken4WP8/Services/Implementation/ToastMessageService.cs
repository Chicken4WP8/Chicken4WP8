using System;
using System.Windows;
using Chicken4WP8.Controls;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.Services.Implementation
{
    public class ToastMessageService : IToastMessageService
    {
        public void HandleMessage(string message, Action complete = null)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    var prompt = new ToastPrompt();
                    prompt.Message = message;
                    if (complete != null)
                        prompt.Completed += (o, e) => complete();

                    prompt.Show();
                });
        }
    }
}
