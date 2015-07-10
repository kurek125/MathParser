using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public class UnknowTokenException : Exception
    {
        public UnknowTokenException() : base("Unknow token")
        {
        }

        public UnknowTokenException(string tokenName) : base($"Token: \"{tokenName}\" is unknow")
        {
        }

        public UnknowTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
