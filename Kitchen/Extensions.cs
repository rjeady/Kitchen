using System.Collections.Generic;
using System.Linq;

namespace Kitchen
{
    public static class Extensions
    {
        public static bool IsAnyOf<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        /// <summary>
        /// Wraps this object instance into an IEnumerable{T} consisting of a single item.
        /// If the object is null, an empty IEnumerable{T} will be returned.
        /// </summary>
        /// <typeparam name="T">Type of the wrapped object.</typeparam>
        /// <param name="item">The object to wrap.</param>
        /// <returns>
        /// An IEnumerable{T} consisting of a single item, or no items if the object is null.
        /// </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            if (item == null)
                yield break;
            yield return item;
        }
    }
}
