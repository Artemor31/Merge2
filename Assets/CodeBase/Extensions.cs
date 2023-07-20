using System.Collections.Generic;
using System.Linq;

namespace CodeBase
{
    public static class Extensions
    {
        public static T Random<T>(this IList<T> collection)
        {
            int range = UnityEngine.Random.Range(0, collection.Count());
            return collection[range];
        }
    
        public static T Random<T>(this T[] collection)
        {
            int range = UnityEngine.Random.Range(0, collection.Length);
            return collection[range];
        }
    }
}