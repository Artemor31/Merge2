using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure
{
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new Exception("null collection");
            }

            int count;
            if (collection is IList<T> list)
            {
                count = list.Count;
                if (count < 1)
                {
                    throw new Exception("Collection is empty");
                }
                return list[UnityEngine.Random.Range(0, count)];
            }
            
            IEnumerable<T> enumerable = collection.ToList();
            count = enumerable.Count();
            if (count < 1)
            {
                throw new Exception("Collection is empty");
            }
            
            return enumerable.ElementAt(UnityEngine.Random.Range(0, count));
        }
        
        public static Vector3 ToV3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
    }
}