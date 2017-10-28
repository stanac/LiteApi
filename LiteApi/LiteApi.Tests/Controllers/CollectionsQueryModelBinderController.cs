using System.Collections.Generic;
using System.Linq;
using static LiteApi.Tests.ParameterParsingTests;

namespace LiteApi.Tests.Controllers
{
    public class CollectionsQueryModelBinderController: LiteController
    {
        public int SumArray(int[] ints) => ints.Sum();

        public int SumList(List<int> ints) => ints.Sum();

        public int SumCollection(IEnumerable<int> ints) => ints.Sum();

        public int SumArrayNullable(int?[] ints) => ints.Select(x => x ?? 0).Sum();

        public int SumListNullable(List<int?> ints) => ints.Select(x => x ?? 0).Sum();

        public int SumCollectionNullable(IEnumerable<int?> ints) => ints.Select(x => x ?? 0).Sum();

        public int SumNotCollection(int ints) => ints;

        public string JoinEnumValues(TestEnum[] e) => string.Join(";", e.Select(x => x.ToString()));
    }
}
