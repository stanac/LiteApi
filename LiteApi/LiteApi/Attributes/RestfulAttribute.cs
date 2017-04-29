using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Maps all actions to root path of controller, unless actions has some attributes to override its path
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RestfulAttribute: Attribute
    {
    }
}
