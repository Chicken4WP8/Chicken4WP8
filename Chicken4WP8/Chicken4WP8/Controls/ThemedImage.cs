using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chicken4WP8.Controls
{
    [TemplatePart(Name = ElementImageBrushName, Type = typeof(ImageBrush))]
    public class ThemedImage : Control
    {
        private const string ElementImageBrushName = "ImageBrush";
        private Image ElementImageBrush;

        public delegate void SourceUrlChangedEventHandler(object sender);
        public event SourceUrlChangedEventHandler SourceUrlChanged;

        public ThemedImage()
        {
            DefaultStyleKey = typeof(ThemedImage);
        }

        #region default image
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ThemedImage), null);

        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }
        #endregion

        #region public Image Url
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(ThemedImage), new PropertyMetadata(ImageSourceUrlChanged));

        public string ImageUrl
        {
            get
            {
                return (string)GetValue(ImageUrlProperty);
            }
            set
            {
                SetValue(ImageUrlProperty, value);
            }
        }
        #endregion

        private static void ImageSourceUrlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var image = sender as ThemedImage;
            if (e.OldValue != null)
                image.ApplySource();
            image.ImageSourceUrlChanged();
        }

        private void ImageSourceUrlChanged()
        {
            if (SourceUrlChanged != null)
            {
                SourceUrlChanged(this);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ElementImageBrush = GetTemplateChild(ElementImageBrushName) as Image;
            ApplySource();
        }

        public void ApplySource(ImageSource image = null)
        {
            if (ElementImageBrush != null)
            {
                ElementImageBrush.Source = image == null ? DefaultImage : image;
                ElementImageBrush.Stretch = Stretch.Fill;
            }
        }
    }
}
