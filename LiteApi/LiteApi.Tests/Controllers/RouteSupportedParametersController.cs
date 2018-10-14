using System;

namespace LiteApi.Tests.Controllers
{
    public class RouteSupportedParametersController : LiteController
    {
        [ActionRoute("/Action_boolean/{p}")]
        public bool Action_boolean(bool p)
        {
            return p;
        }

        [ActionRoute("/Action_string/{p}")]
        public string Action_string(string p)
        {
            return p;
        }

        [ActionRoute("/Action_char/{p}")]
        public char Action_char(char p)
        {
            return p;
        }

        [ActionRoute("/Action_Int16/{p}")]
        public Int16 Action_Int16(Int16 p)
        {
            return p;
        }

        [ActionRoute("/Action_Int32/{p}")]
        public Int32 Action_Int32(Int32 p)
        {
            return p;
        }

        [ActionRoute("/Action_Int64/{p}")]
        public Int64 Action_Int64(Int64 p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt16/{p}")]
        public UInt16 Action_UInt16(UInt16 p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt32/{p}")]
        public UInt32 Action_UInt32(UInt32 p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt64/{p}")]
        public UInt64 Action_UInt64(UInt64 p)
        {
            return p;
        }

        [ActionRoute("/Action_Byte/{p}")]
        public Byte Action_Byte(Byte p)
        {
            return p;
        }

        [ActionRoute("/Action_SByte/{p}")]
        public SByte Action_SByte(SByte p)
        {
            return p;
        }

        [ActionRoute("/Action_decimal/{p}")]
        public decimal Action_decimal(decimal p)
        {
            return p;
        }

        [ActionRoute("/Action_Single/{p}")]
        public Single Action_Single(Single p)
        {
            return p;
        }

        [ActionRoute("/Action_double/{p}")]
        public double Action_double(double p)
        {
            return p;
        }

        [ActionRoute("/Action_DateTime/{p}")]
        public DateTime Action_DateTime(DateTime p)
        {
            return p;
        }

        [ActionRoute("/Action_Guid/{p}")]
        public Guid Action_Guid(Guid p)
        {
            return p;
        }

        [ActionRoute("/Action_boolean_Nullable/{p}")]
        public bool? Action_boolean_Nullable(bool? p)
        {
            return p;
        }

        [ActionRoute("/Action_char_Nullable/{p}")]
        public char? Action_char_Nullable(char? p)
        {
            return p;
        }

        [ActionRoute("/Action_Int16_Nullable/{p}")]
        public Int16? Action_Int16_Nullable(Int16? p)
        {
            return p;
        }

        [ActionRoute("/Action_Int32_Nullable/{p}")]
        public Int32? Action_Int32_Nullable(Int32? p)
        {
            return p;
        }

        [ActionRoute("/Action_Int64_Nullable/{p}")]
        public Int64? Action_Int64_Nullable(Int64? p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt16_Nullable/{p}")]
        public UInt16? Action_UInt16_Nullable(UInt16? p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt32_Nullable/{p}")]
        public UInt32? Action_UInt32_Nullable(UInt32? p)
        {
            return p;
        }

        [ActionRoute("/Action_UInt64_Nullable/{p}")]
        public UInt64? Action_UInt64_Nullable(UInt64? p)
        {
            return p;
        }

        [ActionRoute("/Action_Byte_Nullable/{p}")]
        public Byte? Action_Byte_Nullable(Byte? p)
        {
            return p;
        }

        [ActionRoute("/Action_SByte_Nullable/{p}")]
        public SByte? Action_SByte_Nullable(SByte? p)
        {
            return p;
        }

        [ActionRoute("/Action_decimal_Nullable/{p}")]
        public decimal? Action_decimal_Nullable(decimal? p)
        {
            return p;
        }

        [ActionRoute("/Action_Single_Nullable/{p}")]
        public Single? Action_Single_Nullable(Single? p)
        {
            return p;
        }

        [ActionRoute("/Action_double_Nullable/{p}")]
        public double? Action_double_Nullable(double? p)
        {
            return p;
        }

        [ActionRoute("/Action_DateTime_Nullable/{p}")]
        public DateTime? Action_DateTime_Nullable(DateTime? p)
        {
            return p;
        }

        [ActionRoute("/Action_Guid_Nullable/{p}")]
        public Guid? Action_Guid_Nullable(Guid? p)
        {
            return p;
        }

        [ActionRoute("/Action_DateTimeOffset/{p}")]
        public DateTimeOffset Action_DateTimeOffset(DateTimeOffset p)
        {
            return p;
        }

        [ActionRoute("/Action_DateTimeOffset_Nullable/{p}")]
        public DateTimeOffset? Action_DateTimeOffset_Nullable(DateTimeOffset? p)
        {
            return p;
        }
    }
}
