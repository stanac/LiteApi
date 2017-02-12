using Microsoft.AspNetCore.Http;
using System;
using System.IO;
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
        /// Write response of file download.
        /// </summary>
        /// <param name="data">The data to download.</param>
        /// <param name="contentType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult FileDownload(byte[] data, string contentType, string fileName)
        {
            return new FileDownloadActionResult(data, contentType, fileName);
        }

        /// <summary>
        /// Write response of file download. Provided Stream will not be disposed, you need to do it yourself.
        /// </summary>
        /// <param name="data">The data to download.</param>
        /// <param name="contentType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult FileDownload(Stream data, string contentType, string fileName)
        {
            return new FileDownloadActionResult(data, contentType, fileName);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => "CTRL: " + GetType().FullName;
    }
}
