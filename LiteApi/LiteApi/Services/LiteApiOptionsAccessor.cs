using LiteApi.Contracts.Abstractions;
using System;

namespace LiteApi.Services
{
    /// <summary>
    /// Retrieves middleware options
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.ILiteApiOptionsAccessor" />
    public class LiteApiOptionsAccessor : ILiteApiOptionsAccessor
    {
        private readonly LiteApiOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Middleware options</param>
        public LiteApiOptionsAccessor(LiteApiOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Gets instance of <see cref="LiteApiOptions" /> (middleware options)
        /// </summary>
        /// <returns>Instance of <see cref="LiteApiOptions" /></returns>
        public LiteApiOptions GetOptions() => _options;
    }
}
