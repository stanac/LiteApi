using System;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromUrlAttribute : Attribute
    {
    }
}
