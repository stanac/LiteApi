using LiteApi.Contracts.Abstractions;
using System;

namespace LiteApi.Services
{
    /// <summary>
    /// Retrieves middleware options
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.ILiteApiOptionsRetriever" />
    public class LiteApiOptionsRetriever : ILiteApiOptionsRetriever
    {
        private readonly LiteApiOptions _options;

        public LiteApiOptionsRetriever(LiteApiOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public LiteApiOptions GetOptions() => _options;
    }
}
