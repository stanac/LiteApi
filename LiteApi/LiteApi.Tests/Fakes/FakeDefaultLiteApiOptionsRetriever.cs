using LiteApi.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteApi.Tests.Fakes
{
    class FakeDefaultLiteApiOptionsRetriever : ILiteApiOptionsAccessor
    {
        public LiteApiOptions GetOptions()
        {
            return LiteApiOptions.Default;
        }
    }
}
