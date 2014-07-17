using System.Windows;
using System.Windows.Controls;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.Controls
{
    public abstract class TemplateSelector : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ContentTemplate = this.SelectTemplate(this, newContent);
        }

        protected virtual DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            return null;
        }
    }

    public class StatusTemplateSelector : TemplateSelector
    {
        public DataTemplate StatusTemplate { get; set; }

        public DataTemplate RetweetTemplate { get; set; }

        protected override DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            var tweet = newValue as ITweetModel;
            if (tweet.RetweetedStatus == null)
                return StatusTemplate;
            return RetweetTemplate;
        }
    }

    public class StatusDetailTemplateSelector : StatusTemplateSelector
    {
        public DataTemplate StatusDetailTemplate { get; set; }

        public DataTemplate RetweetDetailTemplate { get; set; }

        protected override DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            var tweet = newValue as ITweetModel;
            if (tweet.IsStatusDetail)
            {
                if (tweet.RetweetedStatus != null)
                    return RetweetDetailTemplate;
                return StatusDetailTemplate;
            }
            return base.SelectTemplate(sender, newValue);
        }
    }
}
