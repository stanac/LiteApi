using LiteApi.Contracts.Abstractions;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace LiteApi.Contracts.Models
{
    public class ActionContext
    {
        // public Guid ActionGuid { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public ActionParameter[] Parameters { get; set; }
        public SupportedHttpMethods HttpMethod { get; set; }
        public MethodInfo Method { get; set; }
        public ControllerContext ParentController { get; set; }
        public IApiFilter[] Filters { get; set; } = new IApiFilter[0];
        public bool SkipAuth { get; set; }

    }
}
