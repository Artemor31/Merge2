﻿using System;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.ValidatorAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : ValidatorAttribute
    {
        public string Message { get; private set; }

        public RequiredAttribute(string message = null)
        {
            Message = message;
        }
    }
}
