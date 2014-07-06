using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chicken4WP8.Controls
{
    public class ThemedImage : Control
    {
        private const string ElementImageBrushName = "ImageBrush";
        private Image ElementImageBrush;

        public ThemedImage()
        {
            DefaultStyleKey = typeof(ThemedImage);
        }

        #region Image Source
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ThemedImage), new PropertyMetadata(SourceChanged));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var image = sender as ThemedImage;
            image.ApplyImageSource();
        }
        #endregion

        #region Default Image Source
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ThemedImage), null);

        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ElementImageBrush = GetTemplateChild(ElementImageBrushName) as Image;
            ApplyImageSource();
        }

        private void ApplyImageSource()
        {
            if (ElementImageBrush != null)
            {
                ElementImageBrush.Source = Source == null ? DefaultImage : Source;
                ElementImageBrush.Stretch = Stretch.Fill;
            }
        }
    }
}
