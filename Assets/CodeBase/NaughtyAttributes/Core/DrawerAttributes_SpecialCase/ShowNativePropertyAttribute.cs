using System;

namespace CodeBase.NaughtyAttributes.Core.DrawerAttributes_SpecialCase
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ShowNativePropertyAttribute : SpecialCaseDrawerAttribute
    {
    }
}
