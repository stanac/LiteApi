using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.Tests.ModelSetup
{
    public static class UserSetup
    {

        public static ClaimsPrincipal GetUser(params string[] roles)
        {
            var claims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            var user = new ClaimsPrincipal();
            user.AddIdentity(new ClaimsIdentity(claims, "test_auth"));

            return user;
        }

        public static ClaimsPrincipal GetUserWithClaims(params string[] claims)
        {
            var claimValues = claims.Select(x =>
            {
                string[] values = x.Split(':');
                if (values.Length != 2) throw new Exception();
                return new Claim(values[0], values[1]);
            });
            var user = new ClaimsPrincipal();
            user.AddIdentity(new ClaimsIdentity(claimValues, "test_auth"));
            return user;
        }

        public static IAuthorizationPolicyStore GetPolicyStore()
        {
            IAuthorizationPolicyStore store = new AuthorizationPolicyStore();
            store.SetPolicy("Over16", user =>
            {
                var claim = user.Claims.FirstOrDefault(x => x.Type == "age");
                if (claim != null)
                {
                    int val;
                    if (int.TryParse(claim.Value, out val))
                    {
                        return val >= 16;
                    }
                }
                return false;
            });
            store.SetPolicy("Over18", user =>
            {
                var claim = user.Claims.FirstOrDefault(x => x.Type == "age");
                if (claim != null)
                {
                    int val;
                    if (int.TryParse(claim.Value, out val))
                    {
                        return val >= 18;
                    }
                }
                return false;
            });
            return store;
        }
    }
}
