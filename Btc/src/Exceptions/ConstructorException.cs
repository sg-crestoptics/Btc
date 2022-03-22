using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Btc.Exceptions
{
    internal class ConstructorException : Exception
    {
        public ConstructorException()
        {
        }

        public ConstructorException(string? message) : base(message)
        {
        }

        public ConstructorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
