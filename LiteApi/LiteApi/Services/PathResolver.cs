using System;
using Microsoft.AspNetCore.Http;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

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
