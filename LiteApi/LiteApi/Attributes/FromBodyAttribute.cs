using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP body.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromBodyAttribute : Attribute
    {
    }
}
