using System;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Used to set root of the URL for a controller (if not set controller root URL is "api", so controller named "UserController" will respond to URL "/api/user/{action}". If you don't want to use URL root, set value to null or empty string.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class UrlRootAttribute : Attribute
    {
        /// <summary>
        /// Gets the URL root. Set null or empty string if you don't want to use URL root, default value is "api".
        /// </summary>
        /// <value>
        /// The URL root. Set null or empty string if you don't want to use URL root, default value is "api".
        /// </value>
        public string UrlRoot { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlRootAttribute"/> class.
        /// </summary>
        /// <param name="urlRoot">The URL root.</param>
        public UrlRootAttribute(string urlRoot)
        {
            urlRoot = urlRoot ?? "";
            UrlRoot = urlRoot.Replace("\\", "/").TrimStart('/').TrimEnd('/');
        }
    }
}
