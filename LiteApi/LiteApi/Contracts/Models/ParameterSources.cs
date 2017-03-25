namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Source of action parameters, query, body, unknown
    /// </summary>
    public enum ParameterSources
    {
        /// <summary>
        /// Source is query
        /// </summary>
        Query,

        /// <summary>
        /// Source is body
        /// </summary>
        Body,

        /// <summary>
        /// Source is route segment
        /// </summary>
        RouteSegment,

        /// <summary>
        /// Source is dependency injection container
        /// </summary>
        Service,

        /// <summary>
        /// Source us HTTP header in request
        /// </summary>
        Header,

        /// <summary>
        /// Source is unknown
        /// </summary>
        Unknown
    }
}
