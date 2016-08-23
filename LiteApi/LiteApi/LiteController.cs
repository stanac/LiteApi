using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace LiteApi
{
    /// <summary>
    /// Base class for all controllers that should be registered by the middleware
    /// </summary>
    public abstract class LiteController
    {
        private HttpContext _httpContext;

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>>
        public HttpContext HttpContext { get; internal set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ClaimsPrincipal User => HttpContext?.User;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => "CTRL: " + GetType().FullName;
    }
}
