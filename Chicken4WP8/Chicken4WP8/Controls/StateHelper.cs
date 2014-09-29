using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chicken4WP8.Controls
{
    /// <summary>
    /// Copied from http://stackoverflow.com/questions/6002046/binding-visualstatemanager-view-state-to-a-mvvm-viewmodel
    /// </summary>
    public class StateHelper : DependencyObject
    {
        public static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
            "State", typeof(String), typeof(StateHelper), new PropertyMetadata("Normal", StateChanged));


        internal static void StateChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            string newState = (string)args.NewValue;
            if (args.NewValue != null)
            {
                bool res = VisualStateManager.GoToState((Control)target, newState, true);
            }
        }

        public static void SetState(DependencyObject obj, string value)
        {
            obj.SetValue(StateProperty, value);
        }

        public static string GetState(DependencyObject obj)
        {
            return (string)obj.GetValue(StateProperty);
        }
    }
}
