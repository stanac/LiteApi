using LiteApi.Contracts.Abstractions;
using System.Reflection;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Action context, keeps metadata of an action.
    /// </summary>
    public class ActionContext
    {
        private SupportedHttpMethods _httpMethod;
        private string _httpMethodString;
        private MethodInfo _method;

        /// <summary>
        /// Gets a value indicating whether this instance is returning lite action result.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is returning lite action result; otherwise, <c>false</c>.
        /// </value>
        public bool IsReturningLiteActionResult { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => RouteSegments.FirstOrDefault(x => x.IsConstant)?.OriginalValue?.ToLower() ?? "";

        /// <summary>
        /// Gets or sets the action parameters.
        /// </summary>
        /// <value>
        /// The action parameters.
        /// </value>
        public ActionParameter[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the route segments.
        /// </summary>
        /// <value>
        /// The route segments.
        /// </value>
        public RouteSegment[] RouteSegments { get; set; }
        
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public SupportedHttpMethods HttpMethod
        {
            get { return _httpMethod; }
            set
            {
                _httpMethod = value;
                _httpMethodString = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// Gets or sets the reflected method.
        /// </summary>
        /// <value>
        /// The reflected method.
        /// </value>
        public MethodInfo Method
        {
            get { return _method; }
            set
            {
                _method = value;
                IsReturningLiteActionResult = value == null
                    ? false
                    : value.ReturnParameter.ParameterType == typeof(ILiteActionResult)
                        || value.ReturnParameter.ParameterType == typeof(Task<ILiteActionResult>);
            }
        }

        /// <summary>
        /// Gets or sets the parent controller.
        /// </summary>
        /// <value>
        /// The parent controller.
        /// </value>
        public ControllerContext ParentController { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        internal ApiFilterWrapper[] Filters { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether to skip authentication.
        /// </summary>
        /// <value>
        ///   <c>true</c> if skip authentication; otherwise, <c>false</c>. Determined by <see cref="LiteApi.Attributes.SkipFiltersAttribute"/>.
        /// </value>
        public bool SkipAuth { get; set; }

        /// <summary>
        /// Determines whether is HTTP method matched the specified HTTP method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns>Checks if provided httpMethod (as string) is supported by the action</returns>
        public bool IsHttpMethodMatched(string httpMethod) => httpMethod == _httpMethodString;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => 
            $"{ParentController.ControllerType.Name}.{Method.Name}({string.Join(", ", Parameters.Select(x => x.ToString()))})";

        /// <summary>
        /// Initializes filters and possibly other stuff
        /// </summary>
        internal void Init(ILiteApiOptionsRetriever optionsRetriever)
        {
            if (optionsRetriever == null)
            {
                throw new ArgumentNullException(nameof(optionsRetriever));
            }

            if (Filters == null)
            {
                var attribs = Method.GetCustomAttributes().ToArray();
                var apiFilters = attribs
                    .Where(x => typeof(IApiFilter).IsAssignableFrom(x.GetType()))
                    .Cast<IApiFilter>()
                    .ToArray();
                var asyncFilters = attribs
                    .Where(x => typeof(IApiFilterAsync).IsAssignableFrom(x.GetType()))
                    .Cast<IApiFilterAsync>()
                    .ToArray();
                var policyFilters = attribs
                    .Where(x => typeof(IPolicyApiFilter).IsAssignableFrom(x.GetType()))
                    .Cast<IPolicyApiFilter>()
                    .ToArray();

                Filters = apiFilters.Select(x => new ApiFilterWrapper(x))
                    .Union(asyncFilters.Select(x => new ApiFilterWrapper(x)))
                    .Union(policyFilters.Select(x => new ApiFilterWrapper(x, () => optionsRetriever.GetOptions().AuthorizationPolicyStore)))
                    .ToArray();

                SkipAuth = Method
                    .GetCustomAttributes()
                    .Where(x => typeof(Attributes.SkipFiltersAttribute) == x.GetType())
                    .Count() > 0;
            }
        }
    }
}
