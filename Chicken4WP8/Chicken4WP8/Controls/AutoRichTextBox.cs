using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            DependencyProperty.Register("Entities", typeof(IList<IEntity>), typeof(AutoRichTextBox), new PropertyMetadata(OnPropertyChanged));

        public IList<IEntity> Entities
        {
            get { return GetValue(EntitiesProperty) as IList<IEntity>; }
            set { SetValue(EntitiesProperty, value); }
        }
        #endregion

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
            foreach (var entity in Entities.OrderBy(v => v.Begin))
            {
                #region starter
                if (index < entity.Begin)
                {
                    var run = new Run
                    {
                        Text = HttpUtility.HtmlDecode(Text.Substring(index, entity.Begin - index))
                    };
                    paragraph.Inlines.Add(run);
                }
                #endregion
                #region entity
                var hyperlink = new Hyperlink();
                hyperlink.TextDecorations = null;
                hyperlink.Foreground = accentBrush;
                switch (entity.EntityType)
                {
                    #region mention, hashtag,symbol
                    case EntityType.UserMention:
                        hyperlink.CommandParameter = entity;
                        //hyperlink.Click += this.Hyperlink_Click;
                        hyperlink.Inlines.Add("@" + (entity as IUserMentionEntity).ScreenName);
                        break;
                    case EntityType.HashTag:
                        hyperlink.CommandParameter = entity;
                        //hyperlink.Click += this.Hyperlink_Click;
                        hyperlink.Inlines.Add("#" + (entity as ISymbolEntity).Text);
                        break;
                    case EntityType.Symbol:
                        hyperlink.CommandParameter = entity;
                        //hyperlink.Click += this.Hyperlink_Click;
                        hyperlink.Inlines.Add("$" + (entity as ISymbolEntity).Text);
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
                        hyperlink.NavigateUri = url.Url;
                        hyperlink.TargetName = "_blank";
                        hyperlink.Inlines.Add(url.TruncatedUrl);
                        break;
                    #endregion
                }
                paragraph.Inlines.Add(hyperlink);
                index = entity.End;
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
    }
}
