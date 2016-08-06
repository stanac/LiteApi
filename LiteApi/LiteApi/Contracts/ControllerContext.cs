using System;

namespace LiteApi.Contracts
{
    public class ControllerContext
    {
        public string Name { get; set; }
        public ActionContext[] Actions { get; set; }
        public Type ControllerType { get; set; }
    }
}
