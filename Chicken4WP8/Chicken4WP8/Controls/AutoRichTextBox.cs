using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.Controls
{
    public class AutoRichTextBox : RichTextBox
    {
        private static Brush accentBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;

        #region text property
        public static DependencyProperty DataProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoRichTextBox), new PropertyMetadata(OnPropertyChanged));

        public string Text
        {
            get { return GetValue(DataProperty) as string; }
            set { SetValue(DataProperty, value); }
        }
        #endregion

        #region entity property
        public static DependencyProperty EntitiesProperty =
            DependencyProperty.Register("Entities", typeof(IList<IEntity>), typeof(AutoRichTextBox), null);

        public IList<IEntity> Entities
        {
            get { return GetValue(EntitiesProperty) as IList<IEntity>; }
            set { SetValue(EntitiesProperty, value); }
        }
        #endregion

        public static DependencyProperty IsHyperlinkEnableProperty =
            DependencyProperty.Register("IsHyperlinkEnable", typeof(bool), typeof(AutoRichTextBox), null);

        public bool IsHyperlinkEnable
        {
            get { return (bool)GetValue(IsHyperlinkEnableProperty); }
            set { SetValue(IsHyperlinkEnableProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var box = sender as AutoRichTextBox;
            box.PropertyChanged();
        }

        private void PropertyChanged()
        {
            if (string.IsNullOrEmpty(Text))
                return;
            this.Blocks.Clear();
            var paragraph = new Paragraph();
            #region none
            if (Entities == null || Entities.Count == 0)
            {
                paragraph.Inlines.Add(new Run
                {
                    Text = HttpUtility.HtmlDecode(Text)
                });
                this.Blocks.Add(paragraph);
                return;
            }
            #endregion
            #region add entities
            int index = 0;
            foreach (var entity in Entities.OrderBy(v => v.Index))
            {
                #region starter
                if (index < entity.Index)
                {
                    var run = new Run
                    {
                        Text = HttpUtility.HtmlDecode(Text.Substring(index, entity.Index - index))
                    };
                    paragraph.Inlines.Add(run);
                    index = entity.Index;
                }
                #endregion
                #region entity
                var inline = GetInline(entity);
                paragraph.Inlines.Add(inline);
                index += entity.DisplayText.Length;
                #endregion
            }
            #region ender
            if (index < Text.Length)
            {
                var run = new Run
                {
                    Text = HttpUtility.HtmlDecode(Text.Substring(index, Text.Length - index)),
                };
                paragraph.Inlines.Add(run);
            }
            #endregion
            #endregion
            this.Blocks.Add(paragraph);
        }

        private Inline GetInline(IEntity entity)
        {
            if (IsHyperlinkEnable)
            {
                var hyperlink = new Hyperlink();
                hyperlink.TextDecorations = null;
                hyperlink.Foreground = accentBrush;
                switch (entity.EntityType)
                {
                    #region mention, hashtag,symbol
                    case EntityType.HashTag:
                    case EntityType.Symbol:
                    case EntityType.UserMention:
                        hyperlink.Click += HyperlinkClicked;
                        hyperlink.CommandParameter = entity;
                        hyperlink.Inlines.Add(entity.DisplayText);
                        break;
                    #endregion
                    #region media, url
                    case EntityType.Media:
                        var media = entity as IMediaEntity;
                        hyperlink.NavigateUri = media.MediaUrlHttps;
                        hyperlink.TargetName = "_blank";
                        hyperlink.Inlines.Add(media.TruncatedUrl);
                        break;
                    case EntityType.Url:
                        var url = entity as IUrlEntity;
                        hyperlink.NavigateUri = url.ExpandedUrl;
                        hyperlink.TargetName = "_blank";
                        hyperlink.Inlines.Add(url.TruncatedUrl);
                        break;
                    #endregion
                }
                return hyperlink;
            }
            var run = new Run();
            run.Foreground = accentBrush;
            switch (entity.EntityType)
            {
                #region mention, hashtag,symbol
                case EntityType.HashTag:
                case EntityType.Symbol:
                case EntityType.UserMention:
                    run.Text = entity.DisplayText;
                    break;
                #endregion
                #region media, url
                case EntityType.Media:
                    var media = entity as IMediaEntity;
                    run.Text = media.TruncatedUrl;
                    break;
                case EntityType.Url:
                    var url = entity as IUrlEntity;
                    run.Text = url.TruncatedUrl;
                    break;
                #endregion
            }
            return run;
        }

        private void HyperlinkClicked(object sender, RoutedEventArgs e)
        {
            if (IsHyperlinkEnable && HyperlinkClick != null)
                HyperlinkClick(sender, e);
        }

        public event RoutedEventHandler HyperlinkClick;
    }
}
