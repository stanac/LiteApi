using System;
using System.Threading.Tasks;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LiteApi
{
    /// <summary>
    /// File download response
    /// </summary>
    /// <seealso cref="LiteApi.ILiteActionResult" />
    public class FileDownloadActionResult : ILiteActionResult
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloadActionResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mimeType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        public FileDownloadActionResult(byte[] data, string contentType, string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (data == null) throw new ArgumentNullException(nameof(data));

            Data = data;
            ContentType = contentType;
            FileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloadActionResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Content type header value.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public FileDownloadActionResult(Stream data, string contentType, string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (data == null) throw new ArgumentNullException(nameof(data));

            Data = new byte[data.Length];
            data.Read(Data, 0, Data.Length);
            ContentType = contentType;
            FileName = fileName;
        }

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
            if (httpCtx == null) throw new ArgumentNullException(nameof(httpCtx));

            httpCtx.Response.Headers.Add("Content-Type", ContentType);
            httpCtx.Response.Headers.Add("Content-Disposition", $"attachment; filename={FileName}; filename*=UTF-8''{FileName}");
            return httpCtx.Response.Body.WriteAsync(Data, 0, Data.Length);
        }
    }
}
