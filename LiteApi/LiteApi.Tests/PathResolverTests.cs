using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.Discoverers;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace LiteApi.Tests
{
    public class PathResolverTests
    {
        [Fact]
        public void PathResolver_ZeroControllers_ReturnsNull()
        {
            PathResolver resolver = new PathResolver(new ControllerContext[0]);
            var request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/test1/test2/test3");
            var action = resolver.ResolveAction(request);
            Assert.Null(action);
        }

        [Fact]
        public void PathResolver_ControllerWithDifferentHttpMethods_CanResolveMethodByHttpMethod()
        {
            var ctrlCtx = new Controllers.DifferentHttpMethodsController().GetControllerContextAsArray();

            var resolver = new PathResolver(ctrlCtx);
            var path = "/api/DifferentHttpMethods/Action";
            var request = Fakes.FakeHttpRequest.WithGetMethod().WithPath(path);
            resolver.ResolveAndAssert(request, "default get");

            request.AddQuery("a", "5");
            resolver.ResolveAndAssert(request, "get");

            request.Method = "POST";
            resolver.ResolveAndAssert(request, "post");
            
            request.Method = "pUT";
            resolver.ResolveAndAssert(request, "put");

            request.Method = "DelETe";
            resolver.ResolveAndAssert(request, "delete");
        }

        [Fact]
        public void PathResolver_ControllerWithOverridenActions_CanResolveMethodOverloadedWithDifferentNumberAndTypeOfParameters()
        {
            var ctrlCtx = new Controllers.ActionOverloadController().GetControllerContextAsArray();

            var resolver = new PathResolver(ctrlCtx);
            var path = "/api/ActionOverload/GetString";

            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .WithPath(path)
                .AddQuery("a", "test string");
            resolver.ResolveAndAssert(request, "string a");

            request.ClearQuery().AddQuery("a", "");
            resolver.ResolveAndAssert(request, "string a");

            request.ClearQuery().AddQuery("a", "test string 1").AddQuery("b", "test string 2");
            resolver.ResolveAndAssert(request, "string a, string b");

            request.ClearQuery().AddQuery("a", "1");
            resolver.ResolveAndAssert(request, "int a");

            request.ClearQuery().AddQuery("a", "1").AddQuery("b", "4545");
            resolver.ResolveAndAssert(request, "int a, int b");

            request.ClearQuery().AddQuery("a", "1").AddQuery("b", "2").AddQuery("c", "55");
            resolver.ResolveAndAssert(request, "int a, int b, int c");

            request.ClearQuery().AddQuery("a", "true");
            resolver.ResolveAndAssert(request, "bool a");

            request.ClearQuery().AddQuery("a", " TRUE ").AddQuery("b", "  FALSE");
            resolver.ResolveAndAssert(request, "bool a, bool b");

            request.ClearQuery().AddQuery("a", "{DC6F0519-973B-479F-8777-EB3063984457}");
            resolver.ResolveAndAssert(request, "Guid a");

            request.ClearQuery().AddQuery("a", "{DC6F0519-973B-479F-8777-EB3063984457}").AddQuery("b", "DC6F0519-973B-479F-8777-EB3063984457");
            resolver.ResolveAndAssert(request, "Guid a, Guid b");

            request.ClearQuery().AddQuery("a", $"{System.DateTime.Now}");
            resolver.ResolveAndAssert(request, "DateTime? a");
        }

        [Fact]
        public void PathResolver_DifferentOrNoRootController_CanResolveDifferentOrNoRootAction()
        {
            var ctrlCtx = new ControllerDiscoverer(new ActionDiscoverer(new ParametersDiscoverer(new Moq.Mock<IServiceProvider>().Object)))
                .GetControllers(typeof(Controllers.NoRootController).GetTypeInfo().Assembly);

            var resolver = new PathResolver(ctrlCtx);

            var request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/api/v2/DifferentRoot/Get");
            resolver.ResolveAndAssert(request, "DifferentRootController");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/theApi/simpleDifferentRoot/Get");
            resolver.ResolveAndAssert(request, "SimpleDifferentRoot");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/complex/root/with/multiple/parts/complexRoot/get");
            resolver.ResolveAndAssert(request, "ComplexRoot");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/complex/root/with/multiple/parts/get");
            resolver.ResolveAndAssert(request, "ComplexRoot2");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/noroot/get");
            resolver.ResolveAndAssert(request, "NoRoot");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/differentnoroot/get");
            resolver.ResolveAndAssert(request, "DifferentNoRoot");

            request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/api/NoCtrlInName/get");
            resolver.ResolveAndAssert(request, "NoCtrlInName");
        }
    }
}
