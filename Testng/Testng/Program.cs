using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathExpressions;

namespace Testng
{
    class Program
    {
        static void Main(string[] args)
        {
            Tokens tokens = new Tokens();
            Token newToken = new Token()
            {
                Precedence = 5,
                Symbol = "%",
                action = Procent
            };
            tokens.AddFunction(newToken);
        }

        public static void Procent(Stack<double> operands)
        {
            
        }
    }
}
