using System;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.Utility;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : ShowIfAttributeBase
    {
        public HideIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public HideIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }
    }
}
