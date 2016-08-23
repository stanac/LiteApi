using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi
{
    /// <summary>
    /// Exception that can be thrown during middleware registration when validation fails
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class LiteApiRegistrationException : Exception
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public string[] Errors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteApiRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LiteApiRegistrationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteApiRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public LiteApiRegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteApiRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public LiteApiRegistrationException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors?.ToArray();
        }
    }
}
