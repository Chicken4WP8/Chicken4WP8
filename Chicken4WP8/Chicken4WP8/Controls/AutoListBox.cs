using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chicken4WP8.Common;

namespace Chicken4WP8.Controls
{
    public class AutoListBox : ListBox
    {
        private bool alreadyHookedScrollEvents;
        private bool isScrolling;
        private bool visualTreeCreated;
        private const string ScrollingVisualStateName = "Scrolling";
        private const string ScrollingVisualStateGroupName = "ScrollStates";
        private const string VerticalCompressionVisualStateGroupName = "VerticalCompression";
        private const string VerticalCompressionTopVisualStateName = "CompressionTop";
        private const string VerticalCompressionBottomVisualStateName = "CompressionBottom";
        private const string NoVerticalCompressionVisualStateName = "NoVerticalCompression";
        private ScrollViewer scrollViewer = null;
        private VisualStateGroup scrollViewerVisualStateGroup = null;
        private VisualStateGroup verticalCompressionVisualStateGroup = null;
        private ListStretch direction;

        public event EventHandler<EventArgs> VerticalCompressionTopHandler;
        public event EventHandler<EventArgs> VerticalCompressionBottomHandler;
        public event EventHandler<ReailizedItemEventArgs> ItemReailizedEventHandler;
        public event EventHandler<ReailizedItemEventArgs> ItemUnReailizedEventHandler;

        public override void OnApplyTemplate()
        {
            // Clean up any existing template items
            UnhookScrollingEvents();
            scrollViewer = null;

            // Even though the template is applied, the full visual tree doesn't exist yet, so anyone
            // that relies on it should not do any work
            visualTreeCreated = false;

            // Allow listbox to apply its template
            base.OnApplyTemplate();

            // Verify that this is in fact a proper listbox that can scroll
            scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            // Hook up to the visual tree
            CompleteVisualSetup();

            return size;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (ItemReailizedEventHandler != null)
                ItemReailizedEventHandler(this, new ReailizedItemEventArgs(element, item));
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            if (ItemUnReailizedEventHandler != null)
                ItemUnReailizedEventHandler(this, new ReailizedItemEventArgs(element, item));
        }

        private void ScrollingStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            isScrolling = (e.NewState.Name == ScrollingVisualStateName);
        }

        private void VerticalCompressionStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == VerticalCompressionTopVisualStateName)
            {
                direction = ListStretch.Top;
            }
            if (e.NewState.Name == VerticalCompressionBottomVisualStateName)
            {
                direction = ListStretch.Bottom;
            }
            if (!isScrolling && e.NewState.Name == NoVerticalCompressionVisualStateName)
            {
                switch (direction)
                {
                    case ListStretch.Top:
                        if (VerticalCompressionTopHandler != null)
                            Dispatcher.BeginInvoke(() => VerticalCompressionTopHandler(sender, e));
                        break;
                    case ListStretch.Bottom:
                        if (VerticalCompressionBottomHandler != null)
                            Dispatcher.BeginInvoke(() => VerticalCompressionBottomHandler(sender, e));
                        break;
                    default:
                        break;
                }
                direction = ListStretch.None;
            }
        }

        private void CompleteVisualSetup()
        {
            if (visualTreeCreated)
                return;

            // Hook the scrolling events of the scrollviewer, which will now be created
            HookScrollingEvents();

            visualTreeCreated = true;
        }

        private void HookScrollingEvents()
        {
            // This will crash if called in design mode ;-)
            if (DesignerProperties.IsInDesignTool)
                return;

            // Visual States are always on the first child of the control template
            FrameworkElement element = null;
            try
            {
                element = VisualTreeHelper.GetChild(scrollViewer, 0) as FrameworkElement;
            }
            catch { }

            // No events to hook
            if (element == null)
                return;

            // Get the visual state group that reports scrolling state changes, and hook its event
            scrollViewerVisualStateGroup = element.GetVisualStateGroup(ScrollingVisualStateGroupName);
            if (scrollViewerVisualStateGroup != null)
                scrollViewerVisualStateGroup.CurrentStateChanging += ScrollingStateChanging;
            //hook up compression event
            if (scrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled)
                verticalCompressionVisualStateGroup = this.GetVisualStateGroup(VerticalCompressionVisualStateGroupName);
            else
                verticalCompressionVisualStateGroup = element.GetVisualStateGroup(VerticalCompressionVisualStateGroupName);
            if (verticalCompressionVisualStateGroup != null)
                verticalCompressionVisualStateGroup.CurrentStateChanging += VerticalCompressionStateChanging;
        }

        private void UnhookScrollingEvents()
        {
            // This will crash if called in design mode
            if (DesignerProperties.IsInDesignTool)
                return;

            if (scrollViewerVisualStateGroup != null)
                scrollViewerVisualStateGroup.CurrentStateChanging -= ScrollingStateChanging;

            if (verticalCompressionVisualStateGroup != null)
                verticalCompressionVisualStateGroup.CurrentStateChanged -= VerticalCompressionStateChanging;
        }
    }

    public enum ListStretch
    {
        None,
        Top,
        Bottom,
    }

    public class ReailizedItemEventArgs : EventArgs
    {
        public ReailizedItemEventArgs(DependencyObject element, object item)
        {
            Container = element;
            Item = item;
        }

        public DependencyObject Container { get; private set; }
        public object Item { get; private set; }
    }
}
