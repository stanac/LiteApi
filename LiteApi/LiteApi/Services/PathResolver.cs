using System;
using Microsoft.AspNetCore.Http;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

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
            ActionContext actionCtx = null;
            string path = request.Path.Value.ToLower();
            string urlStart = GetUrlStart(path);
            if (urlStart == null)
            {
                return null;
            }
            foreach (var ctrl in _controllerContrxts)
            {
                actionCtx = ctrl.GetActionByPath(path, urlStart);
                if (actionCtx != null)
                {
                    return actionCtx;
                }
            }
            return null;
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
    }
}
