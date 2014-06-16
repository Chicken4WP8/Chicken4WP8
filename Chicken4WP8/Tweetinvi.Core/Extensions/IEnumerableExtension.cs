using System;
using System.Collections.Generic;
using System.Linq;

namespace Tweetinvi.Core.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtension
    {
        public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void Foreach<T>(this T[] collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static bool ContainsSameObjectsAs<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
        {
            return collection.Count() == collection2.Count() && collection.Except(collection2).IsEmpty();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }
    }
}