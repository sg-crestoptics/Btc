using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Btc.CryptoMath.Exceptions
{
    internal class ExceptionPointNotOnCurve : Exception
    {
        public ExceptionPointNotOnCurve()
        {
        }

        public ExceptionPointNotOnCurve(string? message) : base(message)
        {
        }

        public ExceptionPointNotOnCurve(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExceptionPointNotOnCurve(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
