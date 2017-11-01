namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Action executing parameter
    /// </summary>
    public class ActionExecutingParameter
    {
        /// <summary>
        /// Gets or sets the parameter info.
        /// </summary>
        /// <value>
        /// The parameter context.
        /// </value>
        public ActionParameter ParameterInfo { get; internal set; }

        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        /// <value>
        /// The value of the parameter.
        /// </value>
        public object Value { get; internal set; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName => ParameterInfo?.Name;
    }
}
