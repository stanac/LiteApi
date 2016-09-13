using System;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP query.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromUrlAttribute : Attribute
    {
    }
}
