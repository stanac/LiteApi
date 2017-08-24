using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Controller metadata
    /// </summary>
    public class ControllerContext
    {
        private string _routeAndName;
        private string[] _segments;

        /// <summary>
        /// Gets the URL start. Used to determine if request is matched to this controller.
        /// </summary>
        /// <value>
        /// The URL start. Used to determine if request is matched to this controller.
        /// </value>
        internal string UrlStart { get; private set; }

        /// <summary>
        /// Gets or sets the authentication policy store factory.
        /// </summary>
        /// <value>
        /// The authentication policy store factory.
        /// </value>
        // internal IAuthorizationPolicyStore AuthPolicyStore { get; private set; } = LiteApiMiddleware.Options.AuthorizationPolicyStore;

        /// <summary>
        /// Gets or sets the route and name.
        /// </summary>
        /// <value>
        /// The route and name.
        /// </value>
        public string RouteAndName
        {
            get { return _routeAndName; }
            set
            {
                _routeAndName = value;
                _segments = value.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this controller is restful.
        /// </summary>
        /// <value>
        /// <c>true</c> if this controller is restful; otherwise, <c>false</c>.
        /// </value>
        public bool IsRestful { get; set; }

        /// <summary>
        /// Gets the route segments.
        /// </summary>
        /// <value>
        /// The route segments.
        /// </value>
        public string[] RouteSegments => _segments;
        
        /// <summary>
        /// Gets or sets the actions belonging to this controller.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        public ActionContext[] Actions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the controller.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the controller.
        /// </value>
        public Type ControllerType { get; set; }

        /// <summary>
        /// Gets or sets the filters. Filters are used for filtering requests (e.g. for authentication/authorization)
        /// </summary>
        /// <value>
        /// The filters. Filters are used for filtering requests (e.g. for authentication/authorization)
        /// </value>
        internal ApiFilterWrapper[] Filters { get; set; }

        /// <summary>
        /// Initializes the specified options retriver.
        /// </summary>
        /// <param name="optionsRetriver">The options retriever.</param>
        /// <exception cref="ArgumentNullException">optionsRetriver</exception>
        public void Init(ILiteApiOptionsRetriever optionsRetriver)
        {
            if (optionsRetriver == null)
            {
                throw new ArgumentNullException(nameof(optionsRetriver));
            }

            if (Filters == null)
            {
                var attributes = ControllerType.GetTypeInfo().GetCustomAttributes().ToArray();
                var apiFilters = attributes
                    .Where(x => typeof(IApiFilter).IsAssignableFrom(x.GetType()))
                    .Cast<IApiFilter>()
                    .ToArray();
                var asyncFilters = attributes
                    .Where(x => typeof(IApiFilterAsync).IsAssignableFrom(x.GetType()))
                    .Cast<IApiFilterAsync>()
                    .ToArray();
                var policyFilters = attributes
                    .Where(x => typeof(IPolicyApiFilter).IsAssignableFrom(x.GetType()))
                    .Cast<IPolicyApiFilter>()
                    .ToArray();
                Filters = apiFilters.Select(x => new ApiFilterWrapper(x))
                    .Union(asyncFilters.Select(x => new ApiFilterWrapper(x)))
                    .Union(policyFilters.Select(x => new ApiFilterWrapper(x, () => optionsRetriver.GetOptions().AuthorizationPolicyStore)))
                    .ToArray();
                foreach (var action in Actions)
                {
                    action.Init(optionsRetriver);
                }
            }
        }

        /// <summary>
        /// Gets the actions by path.
        /// </summary>
        /// <param name="requestSegments">Route segments of the request.</param>
        /// <param name="method">HTTP request method.</param>
        /// <returns>Actions matching to the provided path</returns>
        public IEnumerable<ActionContext> GetActionsBySegments(string[] requestSegments, SupportedHttpMethods method)
        {
            if (RequestSegmentsMatchController(requestSegments))
            {
                string[] requestSegmentsWithoutController = requestSegments.Skip(RouteSegments.Length).ToArray();
                var actions = Actions.Where(x => x.RouteSegments.Length == requestSegmentsWithoutController.Length && x.HttpMethod == method).ToArray();
                if (actions.Length > 0)
                {
                    foreach (var action in actions)
                    {
                        if (IsActionMatchedToRequestSegments(action, requestSegmentsWithoutController))
                        {
                            yield return action;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Validates the filters.
        /// </summary>
        /// <param name="httpCtx">The HTTP context.</param>
        /// <returns><see cref="ApiFilterRunResult"/> containing result of the filter run.</returns>
        internal async Task<ApiFilterRunResult> ValidateFilters(HttpContext httpCtx)
        {
            foreach (var filter in Filters)
            {
                var result = await filter.ShouldContinueAsync(httpCtx);
                if (!result.ShouldContinue)
                {
                    return result;
                }
            }
            return new ApiFilterRunResult { ShouldContinue = true };
        }

        private bool RequestSegmentsMatchController(string[] requestSegments)
        {
            if (requestSegments.Length < RouteSegments.Length)
            {
                return false;
            }

            for (int i = 0; i < RouteSegments.Length; i++)
            {
                if (RouteSegments[i] != requestSegments[i])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsActionMatchedToRequestSegments(ActionContext action, string[] requestSegments)
        {
            for (int i = 0; i < action.RouteSegments.Length; i++)
            {
                if (action.RouteSegments[i].IsConstant)
                {
                    if (action.RouteSegments[i].ConstantValue != requestSegments[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
