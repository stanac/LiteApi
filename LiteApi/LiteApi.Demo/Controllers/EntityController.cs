using LiteApi.Attributes;

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
    }
}
