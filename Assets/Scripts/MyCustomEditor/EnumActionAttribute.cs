using System;
using UnityEngine;

namespace MyCustomEditor
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EnumActionAttribute : PropertyAttribute
    {
        public Type EnumType { get; }
        public EnumActionAttribute(Type enumType) => EnumType = enumType;
    }
}
