using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace LiteApi
{
    public abstract class LiteController
    {
        private HttpContext _httpContext;

        public HttpContext HttpContext
        {
            get
            {
                if (IsSingleton) throw new InvalidOperationException("Cannot use HttpContext in singleton controller.");
                return _httpContext;
            }
            internal set
            {
                _httpContext = value;
            }
        }

        public ClaimsPrincipal User
        {
            get
            {
                if (IsSingleton) throw new InvalidOperationException("Cannot use HttpContext in singleton controller.");
                return HttpContext?.User;
            }
        }

        public bool IsSingleton { get; internal set; }

        public override string ToString()
        {
            return "CTRL: " + GetType().FullName;
        }
    }
}
