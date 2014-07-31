using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;

namespace Chicken4WP8.Controls
{
    public class ThemedImage : Control
    {
        private const string ElementImageBrushName = "ImageBrush";
        private Image ElementImageBrush;

        static ThemedImage()
        {
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }

        public ThemedImage()
        {
            DefaultStyleKey = typeof(ThemedImage);
        }

        #region image data
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(byte[]), typeof(ThemedImage), new PropertyMetadata(SourceChanged));

        public byte[] Source
        {
            get { return GetValue(SourceProperty) as byte[]; }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var image = sender as ThemedImage;
            image.ApplyImageSource();
        }
        #endregion

        #region default image source
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ThemedImage), null);

        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }
        #endregion

        #region stretch
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ThemedImage), new PropertyMetadata(Stretch.Fill));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
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
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ElementImageBrush.Stretch = Stretch;
                    if (Source == null)
                    {
                        ElementImageBrush.Source = DefaultImage;
                        return;
                    }
                    #region jpeg/png
                    try
                    {
                        using (var memStream = new MemoryStream(Source))
                        {
                            memStream.Position = 0;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(memStream);
                            ElementImageBrush.Source = bitmapImage;
                        }
                    }
                    #endregion
                    #region others
                    catch (Exception exception)
                    {
                        Debug.WriteLine("set gif image. length: {0}", Source.Length);
                        using (var memStream = new MemoryStream(Source))
                        {
                            memStream.Position = 0;
                            var extendedImage = new ExtendedImage();
                            extendedImage.SetSource(memStream);
                            extendedImage.LoadingCompleted += (o, e) =>
                            {
                                var ei = o as ExtendedImage;
                                ElementImageBrush.Source = ei.ToBitmap();
                            };
                        }
                    }
                    #endregion
                });
            }
        }
    }
}
