using System;
using Microsoft.AspNetCore.Http;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Models.ActionMatchingByParameters;

namespace LiteApi.Services
{
    public class PathResolver : IPathResolver
    {
        private readonly ControllerContext[] _controllerContrxts;

        public PathResolver(ControllerContext[] controllers)
        {
            if (controllers == null) throw new ArgumentNullException(nameof(controllers));
            _controllerContrxts = controllers;
            foreach (var ctrl in _controllerContrxts)
            {
                ctrl.Init();
            }
        }

        public ActionContext ResolvePath(HttpRequest request)
        {
            var actions = GetActionsForPath(request).ToArray();
            if (actions.Length == 1) return actions[0];
            if (actions.Length == 0) return null;
            return ResolveActionContextByQueryParameterTypes(request, actions);
        }

        public IEnumerable<ActionContext> GetActionsForPath(HttpRequest request)
        {
            string path = request.Path.Value.ToLower();
            string urlStart = GetUrlStart(path);
            if (urlStart != null)
            {
                foreach (var ctrl in _controllerContrxts)
                {
                    var actions = ctrl.GetActionsByPath(path, urlStart);
                    if (actions != null)
                    {
                        foreach (var a in actions)
                        {
                            yield return a;
                        }
                    }
                }
            }
        }
        
        private ActionContext ResolveActionContextByQueryParameterTypes(HttpRequest request, ActionContext[] actions)
        {
            PossibleParameterType[] paramTypes = request.GetPossibleParameterTypes().ToArray();
            var actionsWithWeight = actions.Select(action => ActionMatchingWeight.CalculateWeight(action, paramTypes));
            return actionsWithWeight.OrderByDescending(x => x.Weight).FirstOrDefault()?.ActionCtx;
        }

        private string GetUrlStart(string url)
        {
            string[] parts = url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string urlStart = null;
            if (parts.Length == 2) urlStart = "/" + parts[0] + "/";
            else if (parts.Length == 3) urlStart = "/" + parts[0] + "/" + parts[1] + "/";

            if (urlStart != null)
            {
                while (urlStart.Contains("//")) urlStart = urlStart.Replace("//", "/");
            }
            return urlStart;
        }

        private bool ActionMethodMatches(ActionContext action, HttpRequest request) 
            => request.Method.ToLower() == action.HttpMethod.ToString().ToLower();
    }
}
