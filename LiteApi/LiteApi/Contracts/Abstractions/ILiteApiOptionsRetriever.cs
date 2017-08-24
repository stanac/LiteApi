namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Retrieves middleware options
    /// </summary>
    public interface ILiteApiOptionsRetriever
    {
        LiteApiOptions GetOptions();
    }
}
