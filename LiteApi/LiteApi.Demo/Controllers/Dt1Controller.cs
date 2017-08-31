using System;

namespace LiteApi.Demo.Controllers
{
    [Attributes.DateTimeParsingFormat("MM/dd/yyyy")]
    public class Dt1Controller: LiteController
    {
        public long Ticks1(DateTime dt)
        {
            return dt.Ticks;
        }

        [Attributes.DateTimeParsingFormat("yyyy-MM-dd")]
        public long Ticks2(DateTime dt)
        {
            return dt.Ticks;
        }

        [Attributes.DateTimeParsingFormat("yyyy-MM-dd HH:mm:ss")]
        public long Ticks3(DateTime dt)
        {
            return dt.Ticks;
        }

    }
}
