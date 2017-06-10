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
        /// Writes response of file download.
        /// </summary>
        /// <param name="data">The data to download.</param>
        /// <param name="contentType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult FileDownload(byte[] data, string contentType, string fileName) 
            => new FileDownloadActionResult(data, contentType, fileName);

        /// <summary>
        /// Write response of file download. Provided Stream will not be disposed, you need to do it yourself.
        /// </summary>
        /// <param name="data">The data to download.</param>
        /// <param name="contentType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult FileDownload(Stream data, string contentType, string fileName) 
            => new FileDownloadActionResult(data, contentType, fileName);

        /// <summary>
        /// Creates response with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult Content(string data, string contentType)
            => new ContentActionResult(data, contentType);

        /// <summary>
        /// Creates response with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult Content(byte[] data, string contentType)
            => new ContentActionResult(data, contentType);

        /// <summary>
        /// Creates response with the specified JSON content.
        /// </summary>
        /// <param name="jsonContent">Content in JSON format.</param>
        /// <returns>Result that can be handled by the middleware.</returns>
        public ILiteActionResult Json(string jsonContent)
            => new JsonActionResult(jsonContent);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => "CTRL: " + GetType().FullName;
    }
}
