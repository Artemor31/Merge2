﻿using System;

namespace CodeBase.NaughtyAttributes.Core.DrawerAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : DrawerAttribute
    {
    }
}
