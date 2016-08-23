using LiteApi.Contracts.Abstractions;
using System.Reflection;
using System.Linq;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Action context, keeps metadata of an action
    /// </summary>
    public class ActionContext
    {
        private SupportedHttpMethods _httpMethod;
        private string _httpMethodString;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public ActionParameter[] Parameters { get; set; }

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
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        /// 
        public MethodInfo Method { get; set; }

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
        public IApiFilter[] Filters { get; set; } = new IApiFilter[0];

        /// <summary>
        /// Gets or sets a value indicating whether to skip authentication.
        /// </summary>
        /// <value>
        ///   <c>true</c> if skip authentication; otherwise, <c>false</c>.
        /// </value>
        public bool SkipAuth { get; set; }

        /// <summary>
        /// Determines whether [is HTTP method matched] [the specified HTTP method].
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
            $"{ParentController.ControllerType.Name}.{Method.Name}({string.Join(", ", Parameters.Select(x => x.Type.Name + " " + x.Name))})";
    }
}
