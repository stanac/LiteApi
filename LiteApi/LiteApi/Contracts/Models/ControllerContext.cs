using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models
{
    public class ControllerContext
    {
        internal List<KeyValuePair<string, ActionContext>> ActionMappings { get; private set; }
        internal string UrlStart { get; private set; }

        // public Guid ControllerGuid { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public string UrlRoot { get; set; }
        public ActionContext[] Actions { get; set; }
        public Type ControllerType { get; set; }
        public IApiFilter[] Filters { get; set; } = new IApiFilter[0];
        
        public void Init()
        {
            if (ActionMappings == null)
            {
                CreateActionMappingsAndUrlStart();
            }
        }

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
