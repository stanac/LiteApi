using LiteApi.Attributes;

namespace LiteApi.Demo.Controllers
{
    [ControllerRoute("/api/v3/entity")]
    public class EntityController : LiteController
    {
        [ActionRoute("{type}/get/{id}")]
        public object GetEntity(string type, int id)
        {
            return new
            {
                Type = type,
                Id = id
            };
        }
    }
}
