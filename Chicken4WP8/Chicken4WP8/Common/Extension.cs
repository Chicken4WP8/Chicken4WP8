using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Chicken4WP8.Common
{
    public static class Extension
    {
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName)
        {
            IEnumerable<FrameworkElement> selfOrAncestors = root.GetVisualAncestors().PrependWith(root);

            foreach (FrameworkElement element in selfOrAncestors)
            {
                IList groups = VisualStateManager.GetVisualStateGroups(element);
                foreach (VisualStateGroup group in groups)
                {
                    if (group != null && group.Name == groupName)
                        return group;
                }
            }
            return null;
        }

        public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            }
        }

        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> list, T head)
        {
            yield return head;
            foreach (T item in list)
                yield return item;
        }

        public static IEnumerable<T> GetVisualDescendents<T>(this FrameworkElement root, bool allAtSameLevel) where T : FrameworkElement
        {
            bool found = false;
            foreach (FrameworkElement e in root.GetVisualDescendents())
            {
                if (e is T)
                {
                    found = true;
                    yield return e as T;
                }
                else
                {
                    if (found == true && allAtSameLevel == true)
                        yield break;
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            Queue<IEnumerable<FrameworkElement>> toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                IEnumerable<FrameworkElement> children = toDo.Dequeue();
                foreach (FrameworkElement child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
        }
    }
}
