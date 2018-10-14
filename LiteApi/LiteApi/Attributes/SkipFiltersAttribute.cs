using System;

namespace LiteApi
{
    /// <summary>
    /// Tells the middleware to invoke action even in user is not authorized or authenticated on controller level.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipFiltersAttribute : Attribute
    {
    }
}
