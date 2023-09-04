using System;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.ValidatorAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        public float MaxValue { get; private set; }

        public MaxValueAttribute(float maxValue)
        {
            MaxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            MaxValue = maxValue;
        }
    }
}
