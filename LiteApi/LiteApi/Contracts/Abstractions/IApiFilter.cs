using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    public interface IApiFilter
    {
        Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx);
    }

    public class ApiFilterRunResult
    {
        public bool ShouldContinue { get; set; }
        public int? SetResponseCode { get; set; }
    }
}
