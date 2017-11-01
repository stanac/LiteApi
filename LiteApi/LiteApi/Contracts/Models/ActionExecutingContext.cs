using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Information about action execution
    /// </summary>
    public class ActionExecutingContext
    {
        internal ActionExecutingContext(ActionParameter[] parameterInfo, object[] paramValues, HttpContext httpContext, ActionContext actionCtx)
        {
            ActionContext = actionCtx ?? throw new System.ArgumentNullException(nameof(actionCtx));
            HttpContext = httpContext ?? throw new System.ArgumentNullException(nameof(httpContext));
            if (parameterInfo == null)  throw new System.ArgumentNullException(nameof(parameterInfo));
            if (paramValues == null) throw new System.ArgumentNullException(nameof(paramValues));

            if (paramValues.Length != parameterInfo.Length)
                throw new System.ArgumentException($"Length of {nameof(parameterInfo)} and {nameof(paramValues)} do not match");

            Parameters = new ActionExecutingParameter[paramValues.Length];
            for (int i = 0; i < paramValues.Length; i++)
            {
                Parameters[i] = new ActionExecutingParameter
                {
                    ParameterInfo = parameterInfo[i],
                    Value = paramValues[i]
                };
            }
        }

        /// <summary>
        /// Gets the action context.
        /// </summary>
        /// <value>
        /// The action context.
        /// </value>
        public ActionContext ActionContext { get; internal set; }
        
        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        public HttpContext HttpContext { get; internal set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public ActionExecutingParameter[] Parameters { get; internal set; }

        /// <summary>
        /// Gets the controller context.
        /// </summary>
        /// <value>
        /// The controller context.
        /// </value>
        public ControllerContext ControllerContext => ActionContext?.ParentController;
    }
}
