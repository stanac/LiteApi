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
        /// <summary>
        /// Gets the action mappings.
        /// </summary>
        /// <value>
        /// The action mappings.
        /// </value>
        internal List<KeyValuePair<string, ActionContext>> ActionMappings { get; private set; }

        /// <summary>
        /// Gets the URL start. Used to determine if request is matched to this controller.
        /// </summary>
        /// <value>
        /// The URL start. Used to determine if request is matched to this controller.
        /// </value>
        internal string UrlStart { get; private set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL root. URL prefix for controller, default is /api/
        /// </summary>
        /// <value>
        /// The URL root. URL prefix for controller, default is /api/
        /// </value>
        public string UrlRoot { get; set; }

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
        public IApiFilter[] Filters { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            if (ActionMappings == null)
            {
                CreateActionMappingsAndUrlStart();
            }
            if (Filters == null)
            {
                Filters = ControllerType
                    .GetTypeInfo()
                    .GetCustomAttributes()
                    .Where(x => typeof(IApiFilter).IsAssignableFrom(x.GetType()))
                    .Cast<IApiFilter>()
                    .ToArray();
                foreach (var action in Actions)
                {
                    action.Init();
                }
            }
        }

        /// <summary>
        /// Gets the actions by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="urlStart">The URL start.</param>
        /// <returns>Actions matching to the provided path</returns>
        public IEnumerable<ActionContext> GetActionsByPath(string path, string urlStart)
        {
            if (urlStart == UrlStart)
            {
                if (ActionMappings == null)
                {
                    CreateActionMappingsAndUrlStart();
                }
                foreach (var a in ActionMappings.Where(x => x.Key == path).Select(x => x.Value))
                {
                    yield return a;
                }
            }
        }

        /// <summary>
        /// Creates the action mappings and URL start.
        /// </summary>
        private void CreateActionMappingsAndUrlStart()
        {
            string urlRoot = "/";
            if (!string.IsNullOrEmpty(UrlRoot))
            {
                urlRoot = "/" + UrlRoot + "/";
                while (urlRoot.Contains("//"))
                {
                    urlRoot = urlRoot.Replace("//", "/");
                }
            }
            string urlStart = urlRoot.ToLower() + Name.ToLower() + "/";
            UrlStart = urlStart;
            List<KeyValuePair<string, ActionContext>> mappings = new List<KeyValuePair<string, ActionContext>>();
            foreach (var action in Actions)
            {
                string url = urlStart + action.Name.ToLower();
                mappings.Add(new KeyValuePair<string, ActionContext>(url, action));
            }
            ActionMappings = mappings;
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
    }
}
