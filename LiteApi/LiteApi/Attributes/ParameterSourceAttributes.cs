using System;

namespace LiteApi
{
    // todo: refactor with attribute inheritance


    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP query.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromQueryAttribute : Attribute
    {
    }

    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP body.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromRouteAttribute : Attribute
    {
    }

    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP body.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromBodyAttribute : Attribute
    {
    }

    /// <summary>
    /// Tells the middleware that parameter should be retrieved from dependency injection container
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromServicesAttribute : Attribute
    {
    }

    /// <summary>
    /// Tells the middleware that parameter should be retrieved from HTTP request header
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromHeaderAttribute : Attribute
    {
        /// <summary>
        /// Header name that can override parameter name
        /// </summary>
        public string HeaderName { get; private set; }

        /// <summary>
        /// Instantiates new version of <see cref="FromHeaderAttribute"/>
        /// </summary>
        public FromHeaderAttribute() { }

        /// <summary>
        /// Instantiates new version of <see cref="FromHeaderAttribute"/>
        /// </summary>
        /// <param name="headerName">Overrides name of the header to use to retrieve parameter</param>
        public FromHeaderAttribute(string headerName)
        {
            HeaderName = headerName;
        }
    }
}
