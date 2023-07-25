using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Infrastructure
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
        
        public static Vector3 ToV3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
    }
}