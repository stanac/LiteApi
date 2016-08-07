using LiteApi.Contracts.Models;
using System;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : HttpBaseAttribute
    {
        public override SupportedHttpMethods Method => SupportedHttpMethods.Get;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpBaseAttribute
    {
        public override SupportedHttpMethods Method => SupportedHttpMethods.Post;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : HttpBaseAttribute
    {
        public override SupportedHttpMethods Method => SupportedHttpMethods.Put;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpBaseAttribute
    {
        public override SupportedHttpMethods Method => SupportedHttpMethods.Delete;
    }

    public abstract class HttpBaseAttribute : Attribute
    {
        public abstract SupportedHttpMethods Method { get; }
    }
}
