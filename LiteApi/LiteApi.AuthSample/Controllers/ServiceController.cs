using LiteApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.AuthSample.Controllers
{
    
    public class ServiceController : LiteController
    {
        [LiteApi.Attributes.SkipFilters]
        public string Public()
        {
            return "Success!";
        }
        
        // authorized by controller filter
        public string ProtectedWithoutSpecialRolesOrClaims()
        {
            return "Success: " + nameof(ProtectedWithoutSpecialRolesOrClaims);
        }

        
        public string ProtectedWithSpecialRoles()
        {
            return "Success: " + nameof(ProtectedWithSpecialRoles);
        }

        
        public string ProtectedWithSpecialClaims()
        {
            return "Success: " + nameof(ProtectedWithSpecialClaims);
        }

        public string ProtectedWithSpecialRolesAndClaims()
        {
            return "Success: " + nameof(ProtectedWithSpecialRolesAndClaims);
        }

        [HttpPost]
        public string ProtectedPostRequest()
        {
            return "Success: " + nameof(ProtectedPostRequest);
        }


    }
}
