using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Reflection;
using LiteApi.Contracts.Abstractions;

namespace LiteApi.Tests
{
    public class LiteApiOptionsTests
    {
        [Fact]
        public void LiteApiOptionsSetLoggerFactory_NullLoggerFactory_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.SetLoggerFactory(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }

            Assert.True(error);
        }

        [Fact]
        public void LiteApiOptionsSetLoggerFactory_NotNullLoggerFactory_SetsLoggerFactory()
        {
            var options = new LiteApiOptions();

            Assert.Null(options.LoggerFactory);

            options.SetLoggerFactory(new Fakes.FakeLoggerFactory());

            Assert.NotNull(options.LoggerFactory);
        }
        
        [Fact]
        public void LiteApiOptionsAddControllerAssemblies_AddsControllerAssemblies_IncreaseNumberOfAssemblies()
        {
            var options = new LiteApiOptions();
            var assembly = Assembly.GetEntryAssembly();
            int count = options.ControllerAssemblies.Count();
            options.AddControllerAssemblies(new[] { assembly });
            Assert.Equal(options.ControllerAssemblies.Count(), count + 1);
        }

        [Fact]
        public void LiteApiOptionsAddAdditionalQueryModelBinder_NullQueryModelBinder_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.AddAdditionalQueryModelBinder(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void LiteApiOptionsAddAdditionalQueryModelBinder_NotNullQueryModelBinder_AddsQueryModelBinder()
        {
            var options = new LiteApiOptions();
            var modelBinder = new Fakes.FakeQueryModelBinder();
            options.AddAdditionalQueryModelBinder(modelBinder);
            var added = typeof(LiteApiOptions).GetProperty("AdditionalQueryModelBinders", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(options) as List<IQueryModelBinder>;
            Assert.Equal(modelBinder, added[0]);
        }

        [Fact]
        public void LiteApiOptionsAddAuthorizationPolicy_NullPolicy_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.AddAuthorizationPolicy("abc", null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void LiteApiOptionsAddAuthorizationPolicy_NullPolicyName_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.AddAuthorizationPolicy(null, (c) => true);
            }
            catch (ArgumentException ex)
            {
                Assert.True(ex.Message.Contains("cannot be null or empty or whitespace"));
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void LiteApiOptionsAddAuthorizationPolicy_EmptyPolicyName_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.AddAuthorizationPolicy("", (c) => true);
            }
            catch (ArgumentException ex)
            {
                Assert.True(ex.Message.Contains("cannot be null or empty or whitespace"));
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void LiteApiOptionsAddAuthorizationPolicy_WhiteSpacePolicyName_ThrowsException()
        {
            var options = new LiteApiOptions();
            bool error = false;
            try
            {
                options.AddAuthorizationPolicy("   ", (c) => true);
            }
            catch (ArgumentException ex)
            {
                Assert.True(ex.Message.Contains("cannot be null or empty or whitespace"));
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void LiteApiOptionsAddAuthorizationPolicy_ValidPolicy_AddsPolicy()
        {
            var options = new LiteApiOptions();
            options.AddAuthorizationPolicy("asd", (c) => true);
            var policyStore = typeof(LiteApiOptions).GetProperty("AuthorizationPolicyStore", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(options) as IAuthorizationPolicyStore;
            var policy = policyStore.GetPolicy("asd");
            Assert.NotNull(policy);
        }
    }
}
