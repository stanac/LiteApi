using LiteApi.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Services
{
    /// <summary>
    /// Validator for action segments
    /// </summary>
    public static class ActionSegmentsValidator
    {
        /// <summary>
        /// Gets the route segments errors if any.
        /// </summary>
        /// <param name="action">The action context.</param>
        /// <returns>Collection of errors to throw if not valid</returns>
        public static IEnumerable<string> GetRouteSegmentsErrors(ActionContext action)
        {
            if (action.RouteSegments.All(x => x.IsParameter))
            {
                yield return $"Action {action.Name ?? "-null-"}({action.Method}) in controller {action?.ParentController?.RouteAndName}({action?.ParentController}) "
                    + "has 0 constant route segments which is not valid, action route has to have at least one constant segment."; 
            }
            if (action.RouteSegments.Length == 0)
            {
                yield return $"Action {action.Name ?? "-null-"} in controller {action?.ParentController?.RouteAndName} "
                    + "has 0 route segments which is not valid, action route has to have at least one constant segment.";
            }
        }
    }
}
