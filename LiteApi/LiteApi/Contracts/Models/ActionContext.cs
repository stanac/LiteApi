using System.Reflection;

namespace LiteApi.Contracts.Models
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
