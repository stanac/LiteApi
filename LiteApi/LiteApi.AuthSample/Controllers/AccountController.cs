using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.AuthSample.Controllers
{
    public class AccountController : LiteController
    {
        public ClaimsPrincipal Check() => User;

        public async Task<ClaimsPrincipal> LoginWithoutRolesOrClaims()
        {
            var user = GetUser(false, false);
            await HttpContext.SignInAsync(Startup.CookieAuthSchemeKey, user);
            return user;
        }

        public async Task<ClaimsPrincipal> LoginWithRolesAndClaims()
        {
            var user = GetUser(true, true);
            await HttpContext.SignInAsync(Startup.CookieAuthSchemeKey, user);
            return user;
        }

        public async Task<ClaimsPrincipal> LoginWithRolesOnly()
        {
            var user = GetUser(false, true);
            await HttpContext.SignInAsync(Startup.CookieAuthSchemeKey, user);
            return user;
        }

        public async Task<ClaimsPrincipal> LoginWithClaimsOnly()
        {
            var user = GetUser(true, false);
            await HttpContext.SignInAsync(Startup.CookieAuthSchemeKey, user);
            return user;
        }

        [LiteApi.Attributes.RequiresAuthentication]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(Startup.CookieAuthSchemeKey);
        }
        
        private ClaimsPrincipal GetUser(bool addClaims, bool addRoles)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Age", "23"));
            claims.Add(new Claim(ClaimTypes.Name, "Asimov"));
            claims.Add(new Claim(ClaimTypes.Upn, "Asimov"));
            claims.Add(new Claim(ClaimTypes.Email, "asimov@example.com"));
            if (addClaims)
            {
                claims.Add(new Claim("customClaimType1", "true"));
                claims.Add(new Claim("customClaimType2", "correct"));
            }
            if (addRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                claims.Add(new Claim(ClaimTypes.Role, "contentCreator"));
            }

            var identity = new ClaimsIdentity(claims, Startup.CookieAuthSchemeKey);
            return new ClaimsPrincipal(identity);
        }
    }
}
