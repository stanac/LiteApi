using System;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DontMappToApiAttribute : Attribute
    {
    }
}
