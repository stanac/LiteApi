using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace LiteApi
{
    /// <summary>
    /// Returns any HTTP content
    /// </summary>
    /// <seealso cref="LiteApi.ILiteActionResult" />
    public class ContentActionResult : ILiteActionResult
    {
        /// <summary>
        /// HTTP headers to return in the response
        /// </summary>
        public static Dictionary<string, StringValues> Headers { get; private set; } = new Dictionary<string, StringValues>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentActionResult"/> class.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="data">The data.</param>
        public ContentActionResult(byte[] data, string contentType)
        {
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            RawData = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentActionResult"/> class.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="data">The data.</param>
        public ContentActionResult(string data, string contentType) 
            : this(Encoding.UTF8.GetBytes(data), contentType)
        { }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the raw data.
        /// </summary>
        /// <value>
        /// The raw data.
        /// </value>
        public byte[] RawData { get; private set; }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="httpCtx">The HTTP context.</param>
        /// <param name="actionCtx">The action context.</param>
        /// <returns>
        /// Task to await
        /// </returns>
        public Task WriteResponse(HttpContext httpCtx, ActionContext actionCtx)
        {
            if (actionCtx == null) throw new ArgumentNullException(nameof(actionCtx));
            if (httpCtx == null) throw new ArgumentNullException(nameof(httpCtx));
            
            httpCtx.Response.Headers.Add("Content-Type", ContentType);
            foreach (var h in Headers)
            {
                httpCtx.Response.Headers.Add(h.Key, h.Value);
            }
            
            return httpCtx.Response.Body.WriteAsync(RawData, 0, RawData.Length);
        }
    }
}
