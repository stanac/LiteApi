namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Retrieves middleware options
    /// </summary>
    public interface ILiteApiOptionsAccessor
    {
        /// <summary>
        /// Gets instance of <see cref="LiteApiOptions" /> (middleware options)
        /// </summary>
        /// <returns>Instance of <see cref="LiteApiOptions" /></returns>
        LiteApiOptions GetOptions();
    }
}
