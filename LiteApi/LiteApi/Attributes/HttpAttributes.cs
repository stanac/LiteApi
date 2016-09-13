using LiteApi.Contracts.Models;
using System;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Tells the middleware that method should be invoked by HTTP GET method.
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.HttpBaseAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : HttpBaseAttribute
    {
        /// <summary>
        /// HTTP method on which to invoke the action method
        /// </summary>
        public override SupportedHttpMethods Method => SupportedHttpMethods.Get;
    }

    /// <summary>
    /// Tells the middleware that method should be invoked by HTTP POST method.
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.HttpBaseAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpBaseAttribute
    {
        /// <summary>
        /// HTTP method on which to invoke the action method
        /// </summary>
        public override SupportedHttpMethods Method => SupportedHttpMethods.Post;
    }

    /// <summary>
    /// Tells the middleware that method should be invoked by HTTP PUT method.
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.HttpBaseAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : HttpBaseAttribute
    {
        /// <summary>
        /// HTTP method on which to invoke the action method
        /// </summary>
        public override SupportedHttpMethods Method => SupportedHttpMethods.Put;
    }

    /// <summary>
    /// Tells the middleware that method should be invoked by HTTP DELETE method.
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.HttpBaseAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpBaseAttribute
    {
        /// <summary>
        /// HTTP method on which to invoke the action method
        /// </summary>
        public override SupportedHttpMethods Method => SupportedHttpMethods.Delete;
    }

    /// <summary>
    /// Base for HTTP method attributes.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public abstract class HttpBaseAttribute : Attribute
    { 
        /// <summary>
        /// HTTP method on which to invoke the action method
        /// </summary>
        public abstract SupportedHttpMethods Method { get; }
    }
}
