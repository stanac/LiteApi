using System;

namespace LiteApi
{
    /// <summary>
    /// Sets format for parsing <see cref="DateTime"/> and <see cref="DateTimeOffset"/>.
    /// Overrides global setting set by <see cref="LiteApiOptions.SetGlobalDateTimeParsingFormat(string)"/>
    /// When attribute set on action it will override parsing format set on controller.
    /// When set to null, LiteApi will try to parse value regardless of the format.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DateTimeParsingFormatAttribute: Attribute
    {
        public string ParsingFormat { get; private set; }

        public DateTimeParsingFormatAttribute(string format)
        {
            if (format == "") format = null;
            ParsingFormat = format;
        }
    }
}
