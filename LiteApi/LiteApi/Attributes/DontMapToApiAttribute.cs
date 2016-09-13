using System;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Attribute used to tell LiteApi middleware not to map method to API in a controller.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class DontMapToApiAttribute : Attribute
    {
    }
}
