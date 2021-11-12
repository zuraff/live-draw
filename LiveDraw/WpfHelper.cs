using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace AntFu7.LiveDraw
{
    public static class WpfHelper
    {
        /// <summary>
        /// Finds all children of given type that are nested within given parent.
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <returns>Enumeration of children</returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject parent)
           where T : DependencyObject
        {
            if (parent == null)
            {
                yield break;
            }

            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                {
                    yield return t;
                }
                var children = FindChildren<T>(child);
                foreach (var ch in children)
                {
                    yield return ch;
                }
            }
        }
    }
}
