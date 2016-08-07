using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LiteApi
{
    public abstract class LiteController
    {
        public HttpContext HttpContext { get; internal set; }

        public ClaimsPrincipal User => HttpContext?.User;
    }
}
