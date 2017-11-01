using System.Threading.Tasks;
using LiteApi.Attributes;
using LiteApi.Contracts.Models;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace LiteApi.Demo.Controllers
{
    [ControllerRoute("/api/v3/entity")]
    public class EntityController : LiteController
    {
        [ActionRoute("{type}/get/{idValue}")]
        public object GetEntity(string type, int idValue)
        {
            return new
            {
                Type = type,
                Id = idValue
            };
        }

        public bool IsNull(int? id) => !id.HasValue;

        public override async Task<bool> BeforeActionExecution(ActionExecutingContext ctx)
        {
            string paramType = ctx.Parameters.First(x => x.ParameterName.ToLower() == "type").Value as string;
            if (paramType == "dinosaur")
            {
                await ctx.HttpContext.Response.WriteAsync(400, "text/plain", "Entities of type dinosaur are not supported.");
                return false;
            }

            string msg = $"Calling {ctx.ControllerContext.RouteAndName}::{ctx.ActionContext.Name} with params: ";
            msg += string.Join(", ", ctx.Parameters.Select(x => $"{{{x.ParameterName}:{x.Value}}}"));
            Console.WriteLine(msg);
            return true;
        }

        public override Task AfterActionExecuted(ActionExecutingContext ctx, object result)
        {
            string msg = $"Called {ctx.ControllerContext.RouteAndName}::{ctx.ActionContext.Name} with params: ";
            msg += string.Join(", ", ctx.Parameters.Select(x => $"{{{x.ParameterName}:{x.Value}}}"));
            msg += " with result: " + JsonConvert.SerializeObject(result);
            Console.WriteLine(msg);
            return base.AfterActionExecuted(ctx, result);
        }
    }
}
