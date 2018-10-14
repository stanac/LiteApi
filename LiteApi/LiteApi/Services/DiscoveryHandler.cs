using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    /// <summary>
    /// Default implementation of <see cref="IDiscoveryHandler" />
    /// </summary>
    public class DiscoveryHandler : IDiscoveryHandler
    {
        private readonly LiteApiOptions _options;
        private readonly ControllerContext[] _controllers;
        private string _discoveryJson;
        private string _discoveryHtml;
        private IModelBinder _modelBinders;
        private readonly IJsonSerializer _serializer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllers">All controller contexts</param>
        /// <param name="optionsAccessor">Options accessor</param>
        /// <param name="serializer">JSON serializer</param>
        public DiscoveryHandler(
            ControllerContext[] controllers, 
            ILiteApiOptionsAccessor optionsAccessor, 
            IJsonSerializer serializer,
            IModelBinder modelBinders)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _options = optionsAccessor.GetOptions();
            _controllers = controllers ?? throw new ArgumentNullException(nameof(controllers));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _modelBinders = modelBinders ?? throw new ArgumentNullException(nameof(modelBinders));
        }

        /// <summary>
        /// Handles request if needed, returns true if request is handled;
        /// </summary>
        /// <param name="ctx">HTTP context</param>
        /// <returns>True if request is handled</returns>
        public async Task<bool> HandleIfNeeded(HttpContext ctx)
        {
            if (!_options.DiscoveryEnabled || ctx.Request.Method != "GET" || !ctx.Request.Path.StartsWithSegments("/liteapi"))
            {
                return false;
            }

            if (ctx.Request.Path == "/liteapi/info")
            {
                ctx.Response.StatusCode = StatusCodes.Status307TemporaryRedirect;
                ctx.Response.Headers.Add("location", "/liteapi/info.html");
                return true;
            }

            if (ctx.Request.Path == "/liteapi/info.html")
            {
                await HandleHtmlRequest(ctx);
                return true;
            }

            if (ctx.Request.Path == "/liteapi/info.json")
            {
                await HandleJsonRequest(ctx);
                return true;
            }

            return false;
        }

        private async Task HandleJsonRequest(HttpContext ctx)
        {
            string json = _discoveryJson;
            if (json == null)
            {
                json = _serializer.Serialize(GetDiscoveryObject());
                _discoveryJson = json;
            }

            ctx.Response.StatusCode = StatusCodes.Status200OK;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(json);
        }

        private async Task HandleHtmlRequest(HttpContext ctx)
        {
            string html = _discoveryHtml;
            if (html == null)
            {
                var asm = typeof(IDiscoveryHandler).Assembly;
                var name = asm.GetManifestResourceNames().Single(x => x.Contains("Discovery.html"));
                using (Stream s = asm.GetManifestResourceStream(name))
                using (TextReader reader = new StreamReader(s))
                {
                    html = await reader.ReadToEndAsync();
                }
                _discoveryHtml = html;
            }

            ctx.Response.StatusCode = StatusCodes.Status200OK;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync(html);
        }

        private object GetDiscoveryObject()
        {
            Type[] parameterTypes = _controllers
                .SelectMany(x => x.Actions)
                .SelectMany(x => x.Parameters)
                .Where(x => x.IsComplex && x.ParameterSource != ParameterSources.Service)
                .GroupBy(x => x.Type.GetFriendlyName())
                .Select(x => x.First().Type)
                .Distinct()
                .ToArray();
            List<Type> disctinctTypes = parameterTypes.ToList();
            GetAllTypesAndSubtypes(disctinctTypes, 0, parameterTypes);
            disctinctTypes = disctinctTypes.Distinct().ToList();

            List<ParameterTypeDefinition> paramDefinitions = disctinctTypes.Select(GetParameterTypeDefinition).ToList();
            var paramDefDict = new Dictionary<string, object>();
            foreach (var p in paramDefinitions)
            {
                paramDefDict[p.Name] = p;
            }

            var ctrlList = _controllers.Select(c =>
            {
                var actionDict = new Dictionary<string, object>();

                var actions = c.Actions.Select(a =>
                {
                    var parms = a.Parameters.Where(x => x.ParameterSource != ParameterSources.Service).Select(p =>
                            new
                            {
                                p.Name,
                                p.IsNullable,
                                p.IsComplex,
                                p.IsCollection,
                                p.IsCollectionElementTypeNullable,
                                CollectionElementType = p.CollectionElementType?.GetFriendlyName(),
                                p.OverridenName,
                                p.ParameterSource,
                                TypeName = p.Type.GetFriendlyName(),
                                p.HasDefaultValue,
                                p.DefaultValue
                            }).ToList();
                    var paramDict = new Dictionary<string, object>();
                    foreach (var p in parms)
                    {
                        paramDict[p.Name] = p;
                    }

                    return new
                    {
                        Method = a.HttpMethod.ToString().ToUpper(),
                        UsesCustomHandler = a.IsReturningLiteActionResult,
                        a.Name,
                        Route = "/" + a.ParentController.RouteAndName + "/" + string.Join("/", a.RouteSegments.Select(rs => rs.OriginalValue)),
                        Parameters = paramDict
                    };
                });
                foreach (var a in actions)
                {
                    actionDict[(a.Method.ToUpper() + " " + a.Route).Trim()] = a;
                }

                return new
                {
                    Name = "/" + c.RouteAndName,
                    Actions = actionDict
                };
            }).ToList();
            var ctrlDict = new Dictionary<string, object>();
            foreach (var ctrl in ctrlList)
            {
                ctrlDict[ctrl.Name] = ctrl;
            }

            return new
            {
                Controllers = ctrlDict,
                ApiRoot = _options.UrlRoot,
                _options.RequiresHttps,
                Info = "https://liteapi.net",
                ParameterDefinitions = paramDefDict
            };
        }

        private void GetAllTypesAndSubtypes(List<Type> listToFill, int level, params Type[] types)
        {
            if (level < 25)
            {
                foreach (var type in types)
                {
                    if (!listToFill.Contains(type))
                    {
                        listToFill.Add(type);
                    }

                    var propTypes = type
                        .GetProperties()
                        .Select(x => x.PropertyType)
                        .Where(x => !_modelBinders.DoesSupportType(x, ParameterSources.Query))
                        .ToArray();

                    GetAllTypesAndSubtypes(listToFill, level + 1, propTypes);
                }
            }
        }

        private ParameterTypeDefinition GetParameterTypeDefinition(Type type)
        {
            string name = type.GetFriendlyName();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p =>
                    new ParameterDefinitionProperty
                    {
                        Name = p.Name,
                        Type = p.PropertyType.GetFriendlyName(),
                        IsComplex = !_modelBinders.DoesSupportType(p.PropertyType, ParameterSources.Query)
                    }
                    ).ToList();

            var def = new ParameterTypeDefinition
            {
                Name = name
            };
            foreach (var p in props)
            {
                def.Properties[p.Name] = p;
            }
            return def;
        }

        private class ParameterTypeDefinition
        {
            public string Name { get; set; }
            public Dictionary<string, ParameterDefinitionProperty> Properties { get; set; } = new Dictionary<string, ParameterDefinitionProperty>();
        }

        private class ParameterDefinitionProperty
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsComplex { get; set; }
        }
    }
}
