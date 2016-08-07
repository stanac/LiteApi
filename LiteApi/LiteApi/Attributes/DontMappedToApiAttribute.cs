using System;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DontMappedToApiAttribute : Attribute
    {
    }
}
