namespace LiteApi.Contracts
{
    public enum SupportedHttpMethods
    {
        Get, 
        Post,
        Put,
        Delete
    }

    public static class SupportedHttpMethodExtension
    {
        public static bool HasBody(this SupportedHttpMethods method)
            => method == SupportedHttpMethods.Post || method == SupportedHttpMethods.Put;
    }
}
