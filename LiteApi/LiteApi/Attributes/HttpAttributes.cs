using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : Attribute
    { }
}
