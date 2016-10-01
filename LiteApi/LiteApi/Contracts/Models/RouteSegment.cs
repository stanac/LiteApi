using System;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Route segment used for matching actions and controllers
    /// </summary>
    public class RouteSegment
    {
        /// <summary>
        /// Gets the original value from the constructor.
        /// </summary>
        /// <value>
        /// The original value.
        /// </value>
        public string OriginalValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this segment is constant.
        /// </summary>
        /// <value>
        /// <c>true</c> if this segment is constant; otherwise, <c>false</c>, which indicates that segment is parameter.
        /// </value>
        public bool IsConstant { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is parameter, if not it's constant.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is parameter; otherwise, <c>false</c>, which indicates that segment is constant.
        /// </value>
        public bool IsParameter => !IsConstant;

        /// <summary>
        /// Gets the name of the parameter, if segment is not a parameter returns null.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Gets the constant value.
        /// </summary>
        /// <value>
        /// The constant value.
        /// </value>
        public string ConstantValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteSegment"/> class.
        /// </summary>
        /// <param name="segment">The route segment value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public RouteSegment(string segment)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));

            OriginalValue = segment;
            IsConstant = !(OriginalValue.StartsWith("{", StringComparison.Ordinal) && OriginalValue.EndsWith("}", StringComparison.Ordinal));
            if (!IsConstant)
            {
                ParameterName = OriginalValue.TrimStart('{').TrimEnd('}');
            }
            else
            {
                ConstantValue = OriginalValue.ToLower();
            }
        }
    }
}
