using LiteApi.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LiteApi.Contracts
{
    public class ActionContext
    {
        public string Name { get; set; }
        public ActionParameter[] Parameters { get; set; }
        public SupportedHttpMethods HttpMethod { get; set; }
        public MethodInfo Method { get; set; }
        public ControllerContext ParentController { get; set; }
    }
}
