namespace LiteApi.AuthSample.Controllers
{
    [RequiresAuthentication]
    public class ServiceController : LiteController
    {
        [SkipFilters]
        public string Public()
        {
            return "Success!";
        }
        
        // authorized by controller filter
        public string ProtectedWithoutRolesOrClaims()
        {
            return "Success: " + nameof(ProtectedWithoutRolesOrClaims);
        }

        [RequiresRoles("admin", "contentCreator")]
        // [RequiresAnyRole("role1", "role2")]
        public string ProtectedWithRoles()
        {
            return "Success: " + nameof(ProtectedWithRoles);
        }

        [RequiresClaims("customClaimType1", "customClaimType2")]
        // [RequiresAnyClaim("claim1", "claim2")]
        // [RequiresClaimWithAnyValue("claim1", "value1", "value2")]
        // [RequiresClaimWithValues("claim1", "value1", "value2")]
        public string ProtectedWithClaims()
        {
            return "Success: " + nameof(ProtectedWithClaims);
        }

        [RequiresAuthorizationPolicy("AgeOver18")] // policy is defined in Startup.cs
        public string ProtectedWithCustomPolicy()
        {
            return "Success: " + nameof(ProtectedWithCustomPolicy);
        }

        [HttpPost] // authorized by controller filter
        public string ProtectedPostRequest()
        {
            return "Success: " + nameof(ProtectedPostRequest);
        }


    }
}
