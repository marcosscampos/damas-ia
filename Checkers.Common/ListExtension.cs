using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Common
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            var collection = enumerable as ICollection<T>;

            if (enumerable == null) return true;
            if(collection == null) return false;

            return !enumerable.Any();
        }
    }
}
