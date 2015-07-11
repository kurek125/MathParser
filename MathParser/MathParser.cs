using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public class MathParser
    {
        private string expression;

        public MathParser(string expression)
        {
            this.expression = expression;
        }

        private List<Token> Parse()
        {
            InfixToPostfixConverter infixToPostfixConverter = new InfixToPostfixConverter(expression);
            var tokens = infixToPostfixConverter.Translate();

            return tokens;
        }

        public double Calculate(bool useRadians=true)
        {
            List<Token> tokens = Parse();
            Stack<double> operands = new Stack<double>();
            foreach (var op in tokens)
            {
                op.Parse(operands,useRadians);
            }
            return operands.Pop();
        }
    }
}
