using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LiteApi
{
    /// <summary>
    /// Form file collection that can read files from request
    /// </summary>
    public class FormFileCollection
    {
        private HttpRequest _request;
        private IFormFileCollection _files;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFileCollection"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        internal FormFileCollection(HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _request = request;
            if ((request.HasFormContentType || request.Form != null) && request.Form?.Files != null && request.Form.Files.Count > 0)
            {
                _files = request.Form.Files;
            }
            else
            {
                _files = new EmptyFormFileCollection();
            }
        }

        /// <summary>
        /// Gets a value indicating whether request has files from form.
        /// </summary>
        /// <value>
        /// <c>true</c> if request has files from form; otherwise, <c>false</c>.
        /// </value>
        public bool HasFiles => FileCount > 0;

        /// <summary>
        /// Gets the file count.
        /// </summary>
        /// <value>
        /// The file count.
        /// </value>
        public int FileCount => _files.Count;

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public IFormFileCollection Files => _files;

        private class EmptyFormFileCollection : IFormFileCollection
        {
            public IFormFile this[int index]
            {
                get
                {
                    throw new IndexOutOfRangeException();
                }
            }

            public IFormFile this[string name]
            {
                get
                {
                    throw new IndexOutOfRangeException();
                }
            }

            public int Count => 0;

            public IEnumerator<IFormFile> GetEnumerator()
            {
                yield break;
            }

            public IFormFile GetFile(string name)
            {
                throw new IndexOutOfRangeException();
            }

            public IReadOnlyList<IFormFile> GetFiles(string name)
            {
                throw new IndexOutOfRangeException();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
