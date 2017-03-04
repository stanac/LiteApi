using LiteApi.Contracts.Models.ActionMatchingByParameters;
using System;
using System.Collections.Generic;
using Xunit;

namespace LiteApi.Tests
{
    public class TypeWithPriorityTests
    {
        [Fact]
        public void TypeWithPriority_UnsuportedType_Has999Prio()
        {
            var prio = TypeWithPriority.GetTypePriority(typeof(TypeWithPriorityTests));
            Assert.Equal(999, prio);
        }

        [Fact]
        public void TypeWithPriority_NullType_ThrowsException()
        {
            bool error = false;
            try
            {
                var type = new TypeWithPriority(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void TypeWithPriority_ArrayType_HasSamePrioAsElement()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var arrayPrio = TypeWithPriority.GetTypePriority(typeof(int[]));
            Assert.Equal(intPrio, arrayPrio);
        }

        [Fact]
        public void TypeWithPriority_ArrayOfNullableType_HasSamePrioAsNotNullableBaseType()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var arrayPrio = TypeWithPriority.GetTypePriority(typeof(int?[]));
            Assert.Equal(intPrio, arrayPrio);
        }

        [Fact]
        public void TypeWithPriority_ListType_HasSamePrioAsElement()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var listPrio = TypeWithPriority.GetTypePriority(typeof(List<int>));
            Assert.Equal(intPrio, listPrio);
        }

        [Fact]
        public void TypeWithPriority_IEnumerableType_HasSamePrioAsElement()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var collectionType = TypeWithPriority.GetTypePriority(typeof(IEnumerable<int>));
            Assert.Equal(intPrio, collectionType);
        }

        [Fact]
        public void TypeWithPriority_ListNullableType_HasSamePrioAsElement()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var listPrio = TypeWithPriority.GetTypePriority(typeof(List<int?>));
            Assert.Equal(intPrio, listPrio);
        }

        [Fact]
        public void TypeWithPriority_IEnumerableNullableType_HasSamePrioAsElement()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var collectionType = TypeWithPriority.GetTypePriority(typeof(IEnumerable<int?>));
            Assert.Equal(intPrio, collectionType);
        }

        [Fact]
        public void TypeWithPriority_NullableType_HasSamePrioAsBaseType()
        {
            var intPrio = TypeWithPriority.GetTypePriority(typeof(int));
            var nullablePrio = TypeWithPriority.GetTypePriority(typeof(int?));
            Assert.Equal(intPrio, nullablePrio);
        }
    }
}
