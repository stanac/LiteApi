using LiteApi.Contracts.Models;
using LiteApi.OpenApi.Models;
using LiteApi.OpenApi.Models.Definition;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.OpenApi
{
    static class SpecificationBuilder
    {
        public static Specification BuildSpecification(LiteApiMiddleware fromMiddleware, HttpContext httpCtx)
        {
            if (fromMiddleware == null) throw new ArgumentNullException(nameof(fromMiddleware));

            string basePath = "/"  + fromMiddleware.Options.UrlRoot.TrimEnd('/');
            basePath = basePath.Replace("//", "/");
            ControllerContext[] controllers = fromMiddleware.GetControllerContexts();

            var spec = new Specification();
            spec.BasePath = basePath;
            spec.Host = httpCtx.Request.Host.ToString();
            spec.Schemes = new string[] { httpCtx.Request.Scheme };
            spec.Info = new Models.Info.Info
            {
                Description = "JSON API",
                Version = "1.0",
                Title = "JSON API"
            };
            foreach (var def in GetDefinitions(controllers))
            {
                spec.Definitions[def.ActualTypeId] = def;
            }

            return spec;
        }
        
        private static List<Definition> GetDefinitions(ControllerContext[] ctrls)
        {
            List<Definition> defs = new List<Definition>();
            foreach (var action in ctrls.SelectMany(x => x.Actions))
            {
                defs.AddRange(GetDefinitions(action));
            }

            defs = defs.GroupBy(x => x.TypeFullName)
                .Select(x => x.First())
                .ToList();

            foreach (var d in defs)
            {
                while (defs.Count(x => x.ActualTypeId == d.ActualTypeId) > 1)
                {
                    var last = defs.Last(x => x.ActualTypeId == d.ActualTypeId);
                    last.ActualTypeId = NameIncrement.IncrementName(last.ActualTypeId);
                }
            }

            return defs;
        }

        private static IEnumerable<Definition> GetDefinitions(ActionContext action)
        {
            foreach (var param in action.Parameters.Where(x => x.IsComplex))
            {
                if (param.ParameterSource == ParameterSources.Service || param.Type == typeof(FormFileCollection))
                {
                    continue;
                }
                var so = ScehmaObject.FromType(param.Type);
                var definition = new Definition
                {
                    OriginalType = param.Type,
                    TypeFullName = param.Type.FullName,
                    DesiredTypeId = so.DesieredTypeId,
                    ActualTypeId = so.DesieredTypeId
                };
                var props = param.Type.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    definition.Properties[prop.Name] = ScehmaObject.FromType(prop.PropertyType);
                }
                yield return definition;
            }
        }
    }
}
