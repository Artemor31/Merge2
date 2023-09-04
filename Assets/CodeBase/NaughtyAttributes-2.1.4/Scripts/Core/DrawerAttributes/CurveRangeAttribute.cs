using System;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.Utility;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CurveRangeAttribute : DrawerAttribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public EColor Color { get; private set; }

        public CurveRangeAttribute(Vector2 min, Vector2 max, EColor color = EColor.Clear)
        {
            Min = min;
            Max = max;
            Color = color;
        }

        public CurveRangeAttribute(EColor color)
            : this(Vector2.zero, Vector2.one, color)
        {
        }

        public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, EColor color = EColor.Clear)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color)
        {
        }
    }
}
