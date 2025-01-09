using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static UnityEngine.Random;

namespace Infrastructure
{
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            if (collection == null) throw new Exception("null collection");

            if (collection is IList<T> list)
            {
                if (list.Count < 1)
                {
                    throw new Exception("Collection is empty");
                }
                return list[Range(0, list.Count)];
            }
            
            List<T> enumerable = collection.ToList();
            if (enumerable.Count < 1)
            {
                throw new Exception("Collection is empty");
            }
            
            return enumerable[Range(0, enumerable.Count)];
        }

        public static T Random<T>(this IEnumerable<T> collection, Func<T, bool> filter) => 
            collection.Where(filter).Random();

        public static Vector3 ToV3(this Vector2 vector2) => new(vector2.x, 0, vector2.y);

        public static IEnumerable<T> AsCollection<T>(this Enum e)  where T : Enum => Enum.GetValues(e.GetType()).Cast<T>();
    }
}