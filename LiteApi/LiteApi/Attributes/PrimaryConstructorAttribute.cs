using System;

namespace LiteApi
{
    /// <summary>
    /// If more than one constructor is present on a controller this attribute decides which constructor to use. 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Constructor)]
    public class PrimaryConstructorAttribute : Attribute
    {
    }
}
