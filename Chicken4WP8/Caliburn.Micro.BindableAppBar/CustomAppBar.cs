﻿using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Caliburn.Micro.BindableAppBar
{

    public class BindableAppBarButton : Control, IApplicationBarIconButton
    {

        //public int Index { get; set; }

        #region IconUri DependencyProperty

        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.RegisterAttached("IconUri", typeof(Uri), typeof(BindableAppBarButton), new PropertyMetadata(OnIconUriChanged));

        private static void OnIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Disable in designer
            if (Execute.InDesignMode) return;

            if (e.NewValue != e.OldValue)
            {
                var btn = ((BindableAppBarButton)d);
                var uri = e.NewValue as Uri;
                if (uri != null)
                {
                    btn.Button.IconUri = uri;
                }
                else if (e.NewValue != null)
                {
                    btn.Button.IconUri = new Uri(e.NewValue.ToString(), UriKind.Relative);
                }
                else
                {
                    btn.Button.IconUri = null;
                }
            }
        }

        #endregion

        #region Text DependencyProperty

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableAppBarButton), new PropertyMetadata(OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableAppBarButton)d).Button.Text = e.NewValue.ToString();
            }
        }

        #endregion

        #region Visibility DependencyProperty

        public new Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        public new static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(BindableAppBarButton), new PropertyMetadata(OnVisibilityChanged));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var button = ((BindableAppBarButton)d);
                BindableAppBar bar = button.Parent as BindableAppBar;

                if (bar != null) bar.Invalidate();
            }
        }

        #endregion

        public AppBarButton Button { get; set; }

        public event EventHandler Click;

        public BindableAppBarButton()
        {
            Button = new AppBarButton();
            Button.Text = "Text";
            Button.IconUri = new Uri("/", UriKind.Relative);
            Button.Click += AppBarButtonClick;

            // Handle change event because Caliburn calls base Control.IsEnabled,
            // so `new` props don't get called
            IsEnabledChanged += OnIsEnabledChanged;
        }

        void AppBarButtonClick(object sender, EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableAppBarButton)sender).Button.IsEnabled = (bool)e.NewValue;
            }
        }
    }

    public class BindableAppBarMenuItem : Control, IApplicationBarMenuItem
    {

        #region Text DependencyProperty

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableAppBarMenuItem), new PropertyMetadata(OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableAppBarMenuItem)d).MenuItem.Text = e.NewValue.ToString();
            }
        }

        #endregion

        #region Visibility DependencyProperty

        public new Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        public new static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(BindableAppBarMenuItem), new PropertyMetadata(OnVisibilityChanged));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var button = ((BindableAppBarMenuItem)d);
                BindableAppBar bar = button.Parent as BindableAppBar;

                bar.Invalidate();
            }
        }

        #endregion

        public AppBarMenuItem MenuItem { get; set; }

        public event EventHandler Click;

        public BindableAppBarMenuItem()
        {
            MenuItem = new AppBarMenuItem();
            MenuItem.Text = "Text";
            MenuItem.Click += AppBarMenuItemClick;

            // Handle change event because Caliburn calls base Control.IsEnabled,
            // so `new` props don't get called
            IsEnabledChanged += OnIsEnabledChanged;
        }

        private void OnIsEnabledChanged(object s, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableAppBarMenuItem)s).MenuItem.IsEnabled = (bool)e.NewValue;
            }
        }

        void AppBarMenuItemClick(object sender, EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

    }

    [ContentProperty("Buttons")]
    public class BindableAppBar : ItemsControl, IApplicationBar
    {
        // ApplicationBar wrapper
        private readonly static ILog Log = LogManager.GetLog(typeof(BindableAppBar));
        internal readonly ApplicationBar ApplicationBar;
        private Color _originalBackgroundColor;
        private Color _selectedBackgroundColor;
        private Color _selectedForegroundColor;

        public BindableAppBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.StateChanged += ApplicationBarStateChanged;

            // Set default value of dependency property to default value of internal app bar's property
            BackgroundColor = ApplicationBar.BackgroundColor;
            ForegroundColor = ApplicationBar.ForegroundColor;

            Loaded += BindableApplicationBarLoaded;
        }

        void ApplicationBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            if (StateChanged != null)
                StateChanged(this, e);
        }

        protected virtual void OnInvalidated()
        {
            if (Invalidated != null)
                Invalidated(this, EventArgs.Empty);
        }

        void BindableApplicationBarLoaded(object sender, RoutedEventArgs e)
        {

            Log.Info("Loaded: ElementName={0}, DeferLoad={1}, IsVisible={2}", Name, DeferLoad, IsVisible);

            // Store original BG color
            _originalBackgroundColor = ApplicationBar.BackgroundColor;

            // Set original appbar color on load
            ApplicationBar.BackgroundColor = _originalBackgroundColor;

            // Get the page
            var page = this.GetVisualAncestors().OfType<PhoneApplicationPage>().LastOrDefault();

            // If we're not defer-loading, assign the appbar
            if (page != null && !DeferLoad && IsVisible)
                page.ApplicationBar = ApplicationBar;
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            Invalidate();
        }

        public void Invalidate()
        {
            // Clear current buttons
            ApplicationBar.Buttons.Clear();
            ApplicationBar.MenuItems.Clear();

            ApplicationBar.BackgroundColor = _selectedBackgroundColor;
            ApplicationBar.ForegroundColor = _selectedForegroundColor;

            // TODO: Use Index prop to reorder them?
            foreach (BindableAppBarButton button in Items.Where(c => c is BindableAppBarButton && ((BindableAppBarButton)c).Visibility == Visibility.Visible))
            {
                ApplicationBar.Buttons.Add(button.Button);
            }
            foreach (BindableAppBarMenuItem button in Items.Where(c => c is BindableAppBarMenuItem && ((BindableAppBarMenuItem)c).Visibility == Visibility.Visible))
            {
                ApplicationBar.MenuItems.Add(button.MenuItem);
            }

            OnInvalidated();
        }

        #region IsVisible DependencyProperty

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(BindableAppBar), new PropertyMetadata(true, OnVisibleChanged));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var appbar = ((BindableAppBar)d).ApplicationBar;

                appbar.IsVisible = (bool)e.NewValue;

                var page = d.GetVisualAncestors().OfType<PhoneApplicationPage>().FirstOrDefault();

                // Swapping bars?
                if (page != null && appbar.IsVisible && page.ApplicationBar != appbar)
                {
                    page.ApplicationBar = appbar;

                    Log.Info("Set appbar as foreground appbar");
                }
            }
        }

        #endregion

        #region Mode DependencyProperty

        public static readonly DependencyProperty ModeProperty =
          DependencyProperty.RegisterAttached("Mode", typeof(ApplicationBarMode), typeof(BindableAppBar), new PropertyMetadata(ApplicationBarMode.Default, OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableAppBar)d).ApplicationBar.Mode = (ApplicationBarMode)e.NewValue;
            }
        }

        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        #endregion

        #region BarOpacity DependencyProperty

        public double BarOpacity
        {
            get { return (double)GetValue(BarOpacityProperty); }
            set { SetValue(BarOpacityProperty, value); }
        }

        public static readonly DependencyProperty BarOpacityProperty =
            DependencyProperty.Register("BarOpacity", typeof(double), typeof(BindableAppBar), new PropertyMetadata(1.0, BarOpacityChanged));

        private static void BarOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableAppBar)d).ApplicationBar.Opacity = (double)e.NewValue;
        }

        #endregion

        #region IsMenuEnabled DependencyProperty

        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(IsMenuEnabledProperty); }
            set { SetValue(IsMenuEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsMenuEnabledProperty =
            DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(BindableAppBar), new PropertyMetadata(true, IsMenuEnabledChanged));

        private static void IsMenuEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableAppBar)d).ApplicationBar.IsMenuEnabled = (bool)e.NewValue;
        }

        #endregion

        #region BackgroundColor DependencyProperty

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(BindableAppBar), new PropertyMetadata(BackgroundColorChanged));

        private static void BackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableAppBar bab = (BindableAppBar)d;
            Color value = (Color)e.NewValue;

            bab.ApplicationBar.BackgroundColor = value;
            bab._selectedBackgroundColor = value;
        }

        #endregion

        #region ForegroundColor DependencyProperty

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(BindableAppBar), new PropertyMetadata(ForegroundColorChanged));

        private static void ForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableAppBar bab = (BindableAppBar)d;
            Color value = (Color)e.NewValue;

            bab.ApplicationBar.ForegroundColor = value;
            bab._selectedForegroundColor = value;
        }

        #endregion

        /// <summary>
        /// Whether or not to defer loading, e.g. during Pivot/Panorama where there could be multiple appbars declared
        /// </summary>
        public bool DeferLoad { get; set; }

        public double DefaultSize
        {
            get { return ApplicationBar.DefaultSize; }
        }

        public double MiniSize
        {
            get { return ApplicationBar.MiniSize; }
        }

        public IList Buttons
        {
            get { return Items; }

        }

        public IList MenuItems
        {
            get { return Items; }
        }

        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;
        public event EventHandler<EventArgs> Invalidated;
    }
}
