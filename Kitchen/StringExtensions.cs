using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitchen
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the specified string occurs within this string.
        /// A parameter specifies the type of search to use for the specified string.
        /// </summary>
        /// <param name="source">The string to search in.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>true if the value parameter occurs within this string, or if value is the empty string; otherwise, false.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}
