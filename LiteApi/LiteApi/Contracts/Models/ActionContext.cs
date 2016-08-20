using LiteApi.Contracts.Abstractions;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Contracts.Models
{
    public class ActionContext
    {
        private SupportedHttpMethods _httpMethod;
        private string _httpMethodString;

        // public Guid ActionGuid { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public ActionParameter[] Parameters { get; set; }
        public SupportedHttpMethods HttpMethod
        {
            get { return _httpMethod; }
            set
            {
                _httpMethod = value;
                _httpMethodString = value.ToString().ToLower();
            }
        }
        public MethodInfo Method { get; set; }
        public ControllerContext ParentController { get; set; }
        public IApiFilter[] Filters { get; set; } = new IApiFilter[0];
        public bool SkipAuth { get; set; }
        public bool IsHttpMethodMatched(string httpMethod)
        {
            return httpMethod == _httpMethodString;
        }

        public override string ToString() => 
            $"{ParentController.ControllerType.Name}.{Method.Name}({string.Join(", ", Parameters.Select(x => x.Type.Name + " " + x.Name))})";
    }
}
