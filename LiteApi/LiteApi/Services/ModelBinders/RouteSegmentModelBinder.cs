﻿using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Class for parsing parameter values from route segment
    /// </summary>
    internal static class RouteSegmentModelBinder
    {
        /// <summary>
        /// Gets the parameter value, parameter must be from route segment.
        /// </summary>
        /// <param name="actionCtx">The action context.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="request">The request.</param>
        /// <returns>Value of the parsed parameter.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">RouteSegmentQueryModelBinder supports only parameters from route segment.</exception>
        public static object GetParameterValue(ActionContext actionCtx, ActionParameter parameter, HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (actionCtx == null) throw new ArgumentNullException(nameof(actionCtx));
            if (parameter.ParameterSource != ParameterSources.RouteSegment) throw new InvalidOperationException($"{nameof(RouteSegmentModelBinder)} supports only parameters from route segment.");

            string[] segments = request.Path.Value.TrimStart('/').TrimEnd('/').Split('/');
            segments = segments.Skip(actionCtx.ParentController.RouteSegments.Length).ToArray();
            // handle empty string
            if (segments.Length == actionCtx.RouteSegments.Length - 1 && request.Path.Value.EndsWith("/", StringComparison.Ordinal) && parameter.Type == typeof(string))
            {
                return "";
            }
            string stringValue = null;
            string paramName = parameter.Name;
            for (int i = 0; i < actionCtx.RouteSegments.Length; i++)
            {
                if (actionCtx.RouteSegments[i].IsParameter && actionCtx.RouteSegments[i].ParameterName == paramName)
                {
                    if (i >= segments.Length)
                    {
                        throw new Exception($"Route segment for parameter {parameter} in action {actionCtx} not found");
                    }
                    else
                    {
                        stringValue = segments[i];
                        break;
                    }
                }
            }
            // shouldn't reach stringValue == null
            if (stringValue == null) throw new Exception($"Route segment for parameter {parameter} in action {actionCtx} not found");
            return BasicQueryModelBinder.ParseSingleQueryValue(stringValue, parameter.Type, false, parameter.Name, new Lazy<string>(() => parameter.ParentActionContext.ToString()), request.HttpContext);
        }
    }
}
