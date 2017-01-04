using LiteApi.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class AdditionalDataTests
    {
        [Fact]
        public void AdditionalData_SetAdditionalData_CanGetAdditionalData()
        {
            var ad = new AdditionalData();
            ad.SetAdditionalData("1", 11);
            int setData = ad.GetAdditionalDataOrDefault<int>("1");
            Assert.Equal(11, setData);
        }

        [Fact]
        public void AdditionalData_NotAdditionalData_CanGetDefaultValue()
        {
            var ad = new AdditionalData();
            int setData = ad.GetAdditionalDataOrDefault<int>("1");
            Assert.Equal(default(int), setData);
        }
    }
}
