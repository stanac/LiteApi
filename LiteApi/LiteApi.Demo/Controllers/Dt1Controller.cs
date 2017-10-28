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

        // format cannot be specified for DateTimeOffset, that's because DateTimeOffset is used when value needs to be very specific (include timezone)
        // recommended format is: 2017-10-28T09:51:17+07:00 which is percent encoded 2017-10-28T09:51:17%2B07:00
        public long TicksDto(DateTimeOffset dt)
        {
            return dt.Ticks;
        }

    }
}
