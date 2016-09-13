using System;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Tells the middleware to invoke action even in user is not authorized on controller level.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipAuthorizationAttribute : Attribute
    {
    }
}
