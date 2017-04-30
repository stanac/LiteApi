using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresHttpsTests
    {
        private readonly ApiFilterRunResult _shouldNotContinue = new ApiFilterRunResult
        {
            ShouldContinue = false,
            SetResponseCode = 400,
            SetResponseMessage = "Bad request, HTTPS request was expected."
        };

        private readonly ApiFilterRunResult _shouldContinue = ApiFilterRunResult.Continue;

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl without ignore skip, action without skip, returns error")]
        public async Task RequiresHttps_HttpCtrlWithoutIgnoreSkipActionWithoutSkip_ReturnsError()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsWithoutIgnoreSkipController),
                actionName: "Get1",
                expectedResult: _shouldNotContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl without ignore skip, action with skip, returns error")]
        public async Task RequiresHttps_HttpCtrlWithoutIgnoreSkipActionWithSkip_ReturnsError()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsWithoutIgnoreSkipController),
                actionName: "Get2",
                expectedResult: _shouldNotContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl without ignore skip, action without skip, returns continue")]
        public async Task RequiresHttps_HttpsCtrlWithoutIgnoreSkipActionWithoutSkip_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsWithoutIgnoreSkipController),
                actionName: "Get1",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl without ignore skip, action with skip, returns continue")]
        public async Task RequiresHttps_HttpsCtrlWithoutInoreSkipActionWithSkip_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsWithoutIgnoreSkipController),
                actionName: "Get2",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl with ignore skip, action without skip returns error")]
        public async Task RequireHttps_HttpCtrlWithIgnoreSkipActionWithoutSkip_ReturnsError()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsWithIgnoreSkipController),
                actionName: "Get1",
                expectedResult: _shouldNotContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl with ignore skip, action with skip returns continue")]
        public async Task RequireHttps_HttpCtrlWithIgnoreSkipActionWithoutSkip_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsWithIgnoreSkipController),
                actionName: "Get2",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl with ignore skip, action without skip returns continue")]
        public async Task RequireHttps_HttpsCtrlWithIgnoreSkipActionWithoutSkip_ReturnsError()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsWithIgnoreSkipController),
                actionName: "Get1",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl with ignore skip, action with skip returns continue")]
        public async Task RequireHttps_HttpsCtrlWithIgnoreSkipActionWithoutSkip_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsWithIgnoreSkipController),
                actionName: "Get2",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl without RequireHttps, action without RequireHttps, returns continue")]
        public async Task RequireHttps_HttpCtrlWithoutRequireHttpsActionWithoutRequireHttps_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsOnActionController),
                actionName: "Get1",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTP, ctrl without RequireHttps, action with RequireHttps, returns continue")]
        public async Task RequireHttps_HttpCtrlWithoutRequireHttpsActionWithRequireHttps_ReturnsError()
            => await AssertRequireHttps(
                useHttps: false,
                ctrlType: typeof(Controllers.RequiresHttpsOnActionController),
                actionName: "Get2",
                expectedResult: _shouldNotContinue
                );
        
        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl without RequireHttps, action without RequireHttps, returns continue")]
        public async Task RequireHttps_HttpsCtrlWithoutRequireHttpsActionWithoutRequireHttps_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsOnActionController),
                actionName: "Get1",
                expectedResult: _shouldContinue
                );

        [Fact(DisplayName = "RequiresHttps, protocol HTTPS, ctrl without RequireHttps, action with RequireHttps, returns continue")]
        public async Task RequireHttps_HttpsCtrlWithoutRequireHttpsActionWithRequireHttps_ReturnsContinue()
            => await AssertRequireHttps(
                useHttps: true,
                ctrlType: typeof(Controllers.RequiresHttpsOnActionController),
                actionName: "Get2",
                expectedResult: _shouldContinue
                );

        private async Task AssertRequireHttps(bool useHttps, Type ctrlType, string actionName, ApiFilterRunResult expectedResult)
        {
            actionName = actionName.ToLower();
            var ctrl = new Fakes.FakeLimitedControllerDiscoverer(ctrlType).GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == actionName);
            
            var ctx = new Fakes.FakeHttpContext();
            if (useHttps) ctx.Request.Protocol = "HTTPS";
            else ctx.Request.Protocol = "HTTP";

            var result = await ActionInvoker.RunFiltersAndCheckIfShouldContinue(ctx, action);

            Assert.Equal(expectedResult.ShouldContinue, result.ShouldContinue);
            if (!expectedResult.ShouldContinue)
            {
                Assert.Equal(expectedResult.SetResponseCode, result.SetResponseCode);
                Assert.Equal(expectedResult.SetResponseMessage, result.SetResponseMessage);
            }
        }
        
    }
}
