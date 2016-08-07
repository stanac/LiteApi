using System;

namespace LiteApi.Contracts.Models
{
    public class ControllerContext
    {
        public string Name { get; set; }
        public string UrlRoot { get; set; }
        public ActionContext[] Actions { get; set; }
        public Type ControllerType { get; set; }
    }
}
