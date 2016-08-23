namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// HTTP methods by the middleware
    /// </summary>
    public enum SupportedHttpMethods
    {
        /// <summary>
        /// GET HTTP method
        /// </summary>
        Get,
        /// <summary>
        /// POST HTTP method
        /// </summary>
        Post,
        /// <summary>
        /// PUT HTTP method
        /// </summary>
        Put,
        /// <summary>
        /// DELETE HTTP method
        /// </summary>
        Delete
    }

    /// <summary>
    /// Extensions for <see cref="SupportedHttpMethods"/>
    /// </summary>
    public static class SupportedHttpMethodExtension
    {
        /// <summary>
        /// Determines whether this instance supports HTTP body.
        /// </summary>
        /// <param name="method">HTTP method to check.</param>
        /// <returns><c>true</c> if provided HTTP method supports HTTP body, otherwise <c>false</c></returns>
        public static bool HasBody(this SupportedHttpMethods method)
            => method == SupportedHttpMethods.Post || method == SupportedHttpMethods.Put;
    }
}
