using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
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
        /// Sets the response status code.
        /// </summary>
        /// <param name="responseCode">The response status code, if null or not set LiteApi will determine by itself response status code.</param>
        public void SetResponseStatusCode(int? responseCode)
        {
            // TODO: validate the response code
            HttpContext.SetResponseStatusCode(responseCode);
        }

        /// <summary>
        /// Adds the response header.
        /// </summary>
        /// <param name="key">The header key.</param>
        /// <param name="values">The header value(s).</param>
        public void AddResponseHeader(string key, StringValues values)
        {
            var headers = HttpContext.GetResponseHeaders(false);
            headers.Add(key, values);
        }

        /// <summary>
        /// Adds the response headers.
        /// </summary>
        /// <param name="keyValuesPairs">The key values pairs to add.</param>
        public void AddResponseHeaders(IDictionary<string, StringValues> keyValuesPairs)
        {
            var headers = HttpContext.GetResponseHeaders(false);
            foreach (var kvp in keyValuesPairs)
            {
                headers[kvp.Key] = kvp.Value;
            }
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
