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

    //public class StatusDetailTemplateSelector : TweetTemplateSelector
    //{
    //    private readonly IStorageService storageService;

    //    public DataTemplate StatusDetailTemplate { get; set; }

    //    public DataTemplate StatusRetweetDetailTemplate { get; set; }

    //    public StatusDetailTemplateSelector()
    //    {
    //        var container = AppBootstrapper.Container;
    //        this.storageService = container.GetInstance(typeof(IStorageService), null) as IStorageService;
    //    }

    //    protected override DataTemplate SelectTemplate(DependencyObject sender, object newValue)
    //    {
    //        Tweet tweet = newValue as Tweet;
    //        var temp = storageService.GetTempTweet();
    //        if (temp.Id == tweet.Id)
    //        {
    //            if (temp.RetweetStatus != null)
    //                return StatusRetweetDetailTemplate;
    //            return StatusDetailTemplate;
    //        }
    //        return base.SelectTemplate(sender, newValue);
    //    }
    //}
}
