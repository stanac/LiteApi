using LiteApi.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Attribute for renaming action and optionally adding route parameters
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <example>[ActionRoute("{param0}/newActionName/{param1}/{param2}")]</example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ActionRouteAttribute : Attribute
    {
        private readonly string _originalValue;

        /// <summary>
        /// Gets the original value from constructor.
        /// </summary>
        /// <value>
        /// The original value.
        /// </value>
        public string OriginalValue => _originalValue;

        /// <summary>
        /// Gets the route segments.
        /// </summary>
        /// <value>
        /// The segments.
        /// </value>
        public IEnumerable<RouteSegment> RouteSegments => _segments.Select(x => x);

        private RouteSegment[] _segments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRouteAttribute"/> class.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <example>[ActionRoute("{param0}/newActionName/{param1}/{param2}/anotherConstantSegment")]</example>
        public ActionRouteAttribute(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                _segments = new RouteSegment[0];
                return; // validators will do the job on 0 segments
            }
            _originalValue = route;
            _segments = route
                .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new RouteSegment(x.ToLower()))
                .ToArray();
        }
        
    }
}
