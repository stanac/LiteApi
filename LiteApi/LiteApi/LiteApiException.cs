using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi
{
    public class LiteApiRegistrationException : Exception
    {
        public string[] Errors { get; private set; }
        
        public LiteApiRegistrationException(string message) : base(message)
        {
        }

        public LiteApiRegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LiteApiRegistrationException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors?.ToArray();
        }
    }
}
