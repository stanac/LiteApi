//using LiteApi.Contracts.Abstractions;
//using Microsoft.AspNetCore.Http;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace LiteApi.Tests.FilterTests
//{
//    public class GlobalFiltersTest
//    {
//        [Fact]
//        public void GlobalFiltersCanBeInvoked()
//        {
//            HttpContext httpCtx = new Fakes.FakeHttpContext();
//            var apiFilterMock = new Mock<IApiFilter>();
//            apiFilterMock
//                .Setup(x => x.ShouldContinue(httpCtx))
//                .Returns(ApiFilterRunResult.Unauthenticated);
//            IApiFilter apiFilter = apiFilterMock.Object;
//            IServiceProvider serviceProvider = Fakes.FakeServiceProvider.GetServiceProvider();

//            LiteApiMiddleware middleware = new LiteApiMiddleware(
//                null,
//                LiteApiOptions.Default.AddGlobalFilter(apiFilter),
//                serviceProvider);
//            middleware.Invoke(httpCtx).Wait();
//        }
//    }
//}
