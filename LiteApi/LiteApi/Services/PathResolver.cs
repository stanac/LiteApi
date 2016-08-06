using LiteApi.Contracts;
using System;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Services
{
    public class PathResolver : IPathResolver
    {
        public ActionContext ResolvePath(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
