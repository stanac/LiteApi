using System;
using System.Collections.Generic;

namespace LiteApi.Contracts.Models
{
    public class ControllerContext
    {
        internal Dictionary<string, ActionContext> ActionMappings { get; private set; }
        internal string UrlStart { get; private set; }

        public string Name { get; set; }
        public string UrlRoot { get; set; }
        public ActionContext[] Actions { get; set; }
        public Type ControllerType { get; set; }

        public void Init()
        {
            if (ActionMappings != null)
            {
                CreateActionMappingsAndUrlStart();
            }
        }

        public ActionContext GetActionByPath(string path, string urlStart)
        {
            if (urlStart != UrlStart) return null;

            if (ActionMappings == null)
            {
                CreateActionMappingsAndUrlStart();
            }
            ActionContext action;
            ActionMappings.TryGetValue(path, out action);
            return action;
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
            string urlStart = urlRoot + Name.ToLower() + "/";
            UrlStart = urlStart;
            Dictionary<string, ActionContext> mappings = new Dictionary<string, ActionContext>();
            foreach (var action in Actions)
            {
                string url = urlStart + action.Name.ToLower();
                mappings[url] = action;
            }
            ActionMappings = mappings;
        }
    }
}
