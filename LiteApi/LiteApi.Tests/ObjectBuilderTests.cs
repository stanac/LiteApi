using LiteApi.Services;
using Moq;
using System;
using Xunit;

namespace LiteApi.Tests
{
    public class ObjectBuilderTests
    {
        [Fact]
        public void ObjectBuild_NullServiceProvider_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new ObjectBuilder(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void ObjectBuilder_NullType_ThrowsException()
        {
            bool error = false;
            try
            {
                var ob = new ObjectBuilder(GetServiceProviderMock());
                ob.BuildObject(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void ObjectBuilder_TypeWithDefaultConstructor_CanBeBuilt()
        {
            var ob = new ObjectBuilder(GetServiceProviderMock());
            var o = ob.BuildObject<ObjToBuild_DefaultConstructor>();
            Assert.NotNull(o);
        }

        [Fact]
        public void ObjectBuilder_TypeWithDefinedConstructor_CanBeBuilt()
        {
            var ob = new ObjectBuilder(GetServiceProviderMock());
            var o = ob.BuildObject<ObjToBuild_DefinedConstructor>();
            Assert.NotNull(o);
            Assert.Equal(5, o.I);
        }

        [Fact]
        public void ObjectBuilder_TypeWithNotEmptyConstructor_CanBeBuilt()
        {
            var ob = new ObjectBuilder(GetServiceProviderMock());
            var o = ob.BuildObject<ObjToBuild_WithNotEmptyConstructor>();
            Assert.NotNull(o);
            Assert.Equal("AB", o.AB);
        }

        [Fact]
        public void ObjectBuilder_TypeWithTwoConstructorsAndNoPrimaryConstructor_ThrowsException()
        {
            bool error = false;
            try
            {
                var ob = new ObjectBuilder(GetServiceProviderMock());
                var o = ob.BuildObject<ObjToBuild_NoPrimaryConstructor>();
            }
            catch (Exception ex)
            {
                error = ex.Message.StartsWith("Cannot find constructor for", StringComparison.Ordinal);
            }
            Assert.True(error);
        }
        
        [Fact]
        public void ObjectBuilder_TypeWithEmptyPrimaryConstructor_CanBeBuilt()
        {
            var ob = new ObjectBuilder(GetServiceProviderMock());
            var o = ob.BuildObject<ObjToBuild_EmptyPrimaryConstructor>();
            Assert.NotNull(o);
            Assert.Equal("_", o.S);
        }
        
        [Fact]
        public void ObjectBuilder_TypeWithNotEmptyPrimaryConstructor_CanBeBuilt()
        {
            var ob = new ObjectBuilder(GetServiceProviderMock());
            var o = ob.BuildObject<ObjToBuild_NotEmptyPrimaryConstructor>();
            Assert.NotNull(o);
            Assert.Equal("C3", o.S);
        }
        
        private IServiceProvider GetServiceProviderMock()
        {
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns<object>(type =>
            {
                if ((Type)type == typeof(IAddIntStringService) || (Type)type == typeof(AddIntStringService))
                {
                    return new AddIntStringService();
                }
                if ((Type)type == typeof(IStringContactService) || (Type)type == typeof(StringContactService))
                {
                    return new StringContactService();
                }
                return null;
            });
            return mock.Object;
        }

        private interface IAddIntStringService
        {
            string IntSumToString(int a, int b);
        }
        
        private class AddIntStringService : IAddIntStringService
        {
            public string IntSumToString(int a, int b)
            {
                return $"{a + b}";
            }
        }

        private interface IStringContactService
        {
            string Concat(string s1, string s2);
        }

        private class StringContactService : IStringContactService
        {
            public string Concat(string s1, string s2)
            {
                return $"{s1}{s2}";
            }
        }

        private class ObjToBuild_DefaultConstructor
        {

        }

        private class ObjToBuild_DefinedConstructor
        {
            public int I { get; set; }

            public ObjToBuild_DefinedConstructor()
            {
                I = 5;
            }
        }

        private class ObjToBuild_WithNotEmptyConstructor
        {
            public string AB { get; set; }

            public ObjToBuild_WithNotEmptyConstructor(IStringContactService service)
            {
                AB = service.Concat("A", "B");
            }
        }

        private class ObjToBuild_NoPrimaryConstructor
        {
            public int I { get; set; }
            public ObjToBuild_NoPrimaryConstructor() { I = 1; }

#pragma warning disable RECS0154 // Parameter is never used
            public ObjToBuild_NoPrimaryConstructor(IStringContactService service) { I = 2; }
#pragma warning restore RECS0154 // Parameter is never used
        }

        private class ObjToBuild_EmptyPrimaryConstructor
        {
            public string S { get; set; }

            [PrimaryConstructor]
            public ObjToBuild_EmptyPrimaryConstructor()
            {
                S = "_";
            }

            public ObjToBuild_EmptyPrimaryConstructor(IStringContactService stringContactService, IAddIntStringService addIntStringService)
            {
                S = stringContactService.Concat("C", addIntStringService.IntSumToString(1, 2));
            }
        }

        private class ObjToBuild_NotEmptyPrimaryConstructor
        {
            public string S { get; set; }

            public ObjToBuild_NotEmptyPrimaryConstructor()
            {
                S = "_";
            }

            [PrimaryConstructor]
            public ObjToBuild_NotEmptyPrimaryConstructor(IStringContactService stringContactService, IAddIntStringService addIntStringService)
            {
                S = stringContactService.Concat("C", addIntStringService.IntSumToString(1, 2));
            }
        }
    }
}
