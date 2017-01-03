using System;
using Microsoft.AspNetCore.Http;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Models.ActionMatchingByParameters;
using Microsoft.Extensions.Logging;

namespace LiteApi.Services
{
    /// <summary>
    /// Class for resolving which action (if any) should be invoked for the HTTP request.
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IPathResolver" />
    public class PathResolver : IPathResolver
    {
        private readonly ControllerContext[] _controllerContrxts;
        private static readonly char[] _separator = { '/' };

        /// <summary>
        /// Initializes a new instance of the <see cref="PathResolver"/> class.
        /// </summary>
        /// <param name="controllers">Controllers to check.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public PathResolver(ControllerContext[] controllers)
        {
            if (controllers == null) throw new ArgumentNullException(nameof(controllers));

            _controllerContrxts = controllers;
            foreach (var ctrl in _controllerContrxts)
            {
                ctrl.Init();
            }
        }

        /// <summary>
        /// Resolves which action (if any) should be invoked for given HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="logger">Logger to use, can be null</param>
        /// <returns>ActionContext that should be invoked.</returns>
        public ActionContext ResolveAction(HttpRequest request, ILogger logger = null)
        {
            logger?.LogInformation($"Resolving action for request");
            ActionContext[] actions = GetActionsForPathAndMethod(request).ToArray();
            if (actions.Length == 1)
            {
                logger?.LogInformation($"Single action found: {actions[0]}");
                return actions[0];
            }
            if (actions.Length == 0) return null;
            return ResolveActionContextByQueryParameterTypes(request, actions, logger);
        }

        private IEnumerable<ActionContext> GetActionsForPathAndMethod(HttpRequest request)
        {
            string path = request.Path.Value.ToLower();
            string[] segments = path.TrimStart('/').TrimEnd('/').Split(_separator, StringSplitOptions.None);
            var method = (SupportedHttpMethods)Enum.Parse(typeof(SupportedHttpMethods), request.Method, true);
            foreach (var ctrl in _controllerContrxts)
            {
                var actions = ctrl.GetActionsBySegments(segments, method).ToArray();
                foreach (var a in actions)
                {
                    yield return a;
                }
            }
        }
        
        private ActionContext ResolveActionContextByQueryParameterTypes(HttpRequest request, ActionContext[] actions, ILogger logger)
        {
            logger?.LogInformation("Resolving action by parameters overload");
            PossibleParameterType[] paramTypes = request.GetPossibleParameterTypes().ToArray();
            var actionsWithWeight = actions.Select(action => ActionMatchingWeight.CalculateWeight(action, paramTypes))
                .OrderByDescending(x => x.Weight)
                .ToArray();
            var foundAction = actionsWithWeight.FirstOrDefault()?.ActionCtx;
            if (foundAction != null)
            {
                logger?.LogInformation($"Found action by parameter overload: {foundAction}");
            }
            return foundAction;
        }
        
        private bool ActionMethodMatches(ActionContext action, HttpRequest request) 
            => request.Method.ToLower() == action.HttpMethod.ToString().ToLower();
    }
}
