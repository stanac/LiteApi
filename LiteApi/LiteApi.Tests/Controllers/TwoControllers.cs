using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    public class Two : LiteController
    {
        public int GetTheValue(FakeTwoModelOne model)
        {
            return model.I + model.J;
        }

        [HttpDelete]
        public int DeleteTheValue(FakeTwoModelOne model)
        {
            return model.I + model.J;
        }
    }

    public class TwoController : LiteController
    {
        [HttpPost]
        public int PostInt (FakeTwoModelOne m1, FakeTwoModelOne m2)
        {
            return m1.J + m2.I;
        }
    }

    public class FakeTwoModelOne
    {
        public int I { get; set; }
        public int J { get; set; }
    }
}
